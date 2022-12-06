using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mastonet;

public class TimelineHttpStreaming : TimelineStreaming
{
    private string instance;
    private HttpClient client;
    private CancellationTokenSource? cts;

    public TimelineHttpStreaming(StreamingType type, string? param, string instance, string? accessToken)
        : this(type, param, instance, accessToken, DefaultHttpClient.Instance) { }
    public TimelineHttpStreaming(StreamingType type, string? param, string instance, string? accessToken, HttpClient client)
        : base(type, param, accessToken)
    {
        this.client = client;
        this.instance = instance;
    }

    public override async Task Start(TimeSpan? timeout = null, bool restart = true)
    {
        do
        {
            string url = "https://" + instance;
            switch (streamingType)
            {
                case StreamingType.User:
                    url += "/api/v1/streaming/user";
                    break;
                case StreamingType.Public:
                    url += "/api/v1/streaming/public";
                    break;
                case StreamingType.PublicLocal:
                    url += "/api/v1/streaming/public/local";
                    break;
                case StreamingType.Hashtag:
                    url += "/api/v1/streaming/hashtag?tag=" + param;
                    break;
                case StreamingType.HashtagLocal:
                    url += "/api/v1/streaming/hashtag/local?tag=" + param;
                    break;
                case StreamingType.List:
                    url += "/api/v1/streaming/list?list=" + param;
                    break;
                case StreamingType.Direct:
                    url += "/api/v1/streaming/direct";
                    break;
                default:
                    throw new NotImplementedException();
            }

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                using (cts = new CancellationTokenSource())
                {
                    request.Headers.Add("Authorization", "Bearer " + accessToken);
                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        using (var reader = new StreamReader(stream))
                        {
                            string? eventName = null;
                            string? data = null;

                            DateTime lastReceivedValidLine = DateTime.Now;
                            while (true)
                            {
                                var line = await reader.ReadLineAsync();

                                if (string.IsNullOrEmpty(line))
                                {
                                    // reader returned without a line because of BaseStream.ReadTimeout
                                    var timedOut = timeout != null && lastReceivedValidLine.Add(timeout.Value) < DateTime.Now;
                                    if (reader.EndOfStream || timedOut)
                                    {
                                        // it has been too long since a valid line
                                        var timeoutDuration = DateTime.Now.Subtract(lastReceivedValidLine);
                                        throw new TimeoutException($"TimelineHttpStreaming timed out after: {timeoutDuration.ToString()}");
                                    }
                                    else
                                    {
                                        // nothing to do here, we haven't timed out yet
                                        eventName = data = null;
                                        continue;
                                    }
                                }

                                if (line.StartsWith(":"))
                                {
                                    lastReceivedValidLine = DateTime.Now;
                                    eventName = data = null;
                                    continue;
                                }

                                if (line.StartsWith("event: "))
                                {
                                    lastReceivedValidLine = DateTime.Now;
                                    eventName = line.Substring("event: ".Length).Trim();
                                }
                                else if (line.StartsWith("data: "))
                                {
                                    lastReceivedValidLine = DateTime.Now;
                                    data = line.Substring("data: ".Length);
                                    if (eventName != null)
                                    {
                                        SendEvent(eventName, data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (TimeoutException)
            {
                if (!restart)
                    throw;
                else
                    NotifyStreamRestarted();
            }
        } while (restart);
    }

    public override void Stop()
    {
        if (cts != null)
        {
            cts.Cancel();
            cts = null;
        }
    }
}
