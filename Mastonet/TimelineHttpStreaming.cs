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

    public override async Task Start()
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

                    while (true)
                    {
                        var line = await reader.ReadLineAsync();

                        if (string.IsNullOrEmpty(line) || line.StartsWith(":"))
                        {
                            eventName = data = null;
                            continue;
                        }

                        if (line.StartsWith("event: "))
                        {
                            eventName = line.Substring("event: ".Length).Trim();
                        }
                        else if (line.StartsWith("data: "))
                        {
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

    public override void Stop()
    {
        if (cts != null)
        {
            cts.Cancel();
            cts = null;
        }
    }
}
