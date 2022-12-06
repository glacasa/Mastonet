using Mastonet.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mastonet;

public class TimelineWebSocketStreaming : TimelineHttpStreaming
{
    private ClientWebSocket? socket;
    readonly Task<Instance> instanceGetter;
    private const int receiveChunkSize = 512;

    public TimelineWebSocketStreaming(StreamingType type, string? param, string instance, Task<Instance> instanceGetter, string? accessToken)
        : this(type, param, instance, instanceGetter, accessToken, DefaultHttpClient.Instance) { }
    public TimelineWebSocketStreaming(StreamingType type, string? param, string instance, Task<Instance> instanceGetter, string? accessToken, HttpClient client)
        : base(type, param, instance, accessToken, client)
    {
        this.instanceGetter = instanceGetter;
    }

    public override async Task Start(TimeSpan? timeout = null, bool restart = true)
    {
        var instance = await instanceGetter;
        var url = instance?.Urls?.StreamingAPI;

        if (url == null)
        {
            // websocket disabled, fallback to http streaming
            await base.Start();
            return;
        }

        url += "/api/v1/streaming?access_token=" + accessToken;

        switch (streamingType)
        {
            case StreamingType.User:
                url += "&stream=user";
                break;
            case StreamingType.Public:
                url += "&stream=public";
                break;
            case StreamingType.PublicLocal:
                url += "&stream=public:local";
                break;
            case StreamingType.Hashtag:
                url += "&stream=hashtag&tag=" + param;
                break;
            case StreamingType.HashtagLocal:
                url += "&stream=hashtag:local&tag=" + param;
                break;
            case StreamingType.List:
                url += "&stream=list&list=" + param;
                break;
            case StreamingType.Direct:
                url += "&stream=direct";
                break;
            default:
                throw new NotImplementedException();
        }


        byte[] buffer = new byte[receiveChunkSize];
        MemoryStream ms = new MemoryStream();
        var lastValidMessage = DateTime.Now;
        var timedOut = false;
        do
        {
            try
            {
                if (socket == null || socket.State != WebSocketState.Open || socket.CloseStatus != WebSocketCloseStatus.Empty)
                {
                    if (socket != null) { socket.Dispose(); }
                    socket = new ClientWebSocket();
                    await socket.ConnectAsync(new Uri(url), CancellationToken.None);
                }

                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                ms.Write(buffer, 0, result.Count);

                if (result.EndOfMessage)
                {
                    var messageStr = Encoding.UTF8.GetString(ms.ToArray());

                    var message = JsonConvert.DeserializeObject<TimelineMessage>(messageStr);
                    if (message != null)
                    {
                        lastValidMessage = DateTime.Now;
                        SendEvent(message.Event, message.Payload);
                    }
                    ms.Dispose();
                    ms = new MemoryStream();
                }

                timedOut = timeout != null && lastValidMessage.Add(timeout.Value) < DateTime.Now;
                if (timedOut)
                {
                    var timeoutDuration = DateTime.Now.Subtract(lastValidMessage);
                    throw new TimeoutException($"TimelineWebSocketStreaming timed out after: {timeoutDuration.ToString()}");
                }
            } catch (TimeoutException)
            {
                if (!restart)
                    throw;
                else
                    NotifyStreamRestarted();
            }
        }
        while (restart);

        ms.Dispose();

        this.Stop();
    }

    private class TimelineMessage
    {
        public string Event { get; set; } = default!;

        public string Payload { get; set; } = default!;
    }

    public override void Stop()
    {
        if (socket != null)
        {
            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            socket.Dispose();
            socket = null;
        }

        base.Stop();
    }
}
