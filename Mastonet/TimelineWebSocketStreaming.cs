﻿using Mastonet.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mastonet
{
    public class TimelineWebSocketStreaming : TimelineHttpStreaming
    {
        private ClientWebSocket socket;
        readonly Task<Instance> instanceGetter;
        private const int receiveChunkSize = 512;

        public TimelineWebSocketStreaming(StreamingType type, string param, string instance, Task<Instance> instanceGetter, string accessToken)
            : base(type, param, instance, accessToken)
        {
            this.instanceGetter = instanceGetter;
        }

        public override async Task Start()
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

            socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri(url), CancellationToken.None);

            byte[] buffer = new byte[receiveChunkSize];
            while (socket != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        ms.Write(buffer, 0, result.Count);
                        if (result.EndOfMessage)
                            break;
                    }

                    var messageStr = Encoding.UTF8.GetString(ms.ToArray());

                    var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageStr);
                    var eventName = message["event"];
                    var data = message["payload"];
                    SendEvent(eventName, data);
                }
            }

            this.Stop();
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
}
