using Mastonet.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0
using System.Net.WebSockets;
#endif

namespace Mastonet
{
    public class TimelineStreaming
    {
        private readonly StreamingType streamingType;
        private readonly string param;
        private readonly Task<Instance> instanceGetter;
        private readonly string accessToken;
        private HttpClient client;

        public event EventHandler<StreamUpdateEventArgs> OnUpdate;
        public event EventHandler<StreamNotificationEventArgs> OnNotification;
        public event EventHandler<StreamDeleteEventArgs> OnDelete;
        public event EventHandler<StreamFiltersChangedEventArgs> OnFiltersChanged;
        public event EventHandler<StreamConversationEvenTargs> OnConversation;

        internal TimelineStreaming(StreamingType type, Task<Instance> instanceGetter, string accessToken)
        {
            this.streamingType = type;
            this.instanceGetter = instanceGetter;
            this.accessToken = accessToken;
        }

        internal TimelineStreaming(StreamingType type, string param, Task<Instance> instanceGetter, string accessToken)
        {
            this.streamingType = type;
            this.param = param;
            this.instanceGetter = instanceGetter;
            this.accessToken = accessToken;
        }

        public async Task Start()
        {
            var instance = await instanceGetter;
#if NETSTANDARD2_0
            var streamUri = instance?.Urls.StreamingAPI;
            if (streamUri != null)
            {
                await StartWebSocket(streamUri);
            }
            else
            {
                await StartHttp("https://" + instance.Uri);
            }
#else
            await StartHttp("https://" + instance.Uri);
#endif
        }

#if NETSTANDARD2_0
        private ClientWebSocket socket;
        private async Task StartWebSocket(string url)
        {
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

            StringBuilder sb = new StringBuilder();
            while (socket != null)
            {
                byte[] buffer = new byte[receiveChunkSize];

                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                
                var chunk = Encoding.UTF8.GetString(buffer);
                sb.Append(chunk);

                if (result.EndOfMessage)
                {
                    var messageStr = sb.ToString();

                    var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageStr);
                    var eventName = message["event"];
                    var data = message["payload"];
                    SendEvent(eventName, data);

                    sb = new StringBuilder();
                }
            }

            this.Stop();
        }

        private const int receiveChunkSize = 512;
        private async Task Receive()
        {
            byte[] buffer = new byte[receiveChunkSize];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }
        }
#endif

        private async Task StartHttp(string url)
        {
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


            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var stream = await client.GetStreamAsync(url);

            var reader = new StreamReader(stream);

            string eventName = null;
            string data = null;

            while (client != null)
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
                    SendEvent(eventName, data);
                }
            }

            this.Stop();
        }

        private void SendEvent(string eventName, string data)
        {
            switch (eventName)
            {
                case "update":
                    var status = JsonConvert.DeserializeObject<Status>(data);
                    OnUpdate?.Invoke(this, new StreamUpdateEventArgs() { Status = status });
                    break;
                case "notification":
                    var notification = JsonConvert.DeserializeObject<Notification>(data);
                    OnNotification?.Invoke(this, new StreamNotificationEventArgs() { Notification = notification });
                    break;
                case "delete":
                    var statusId = long.Parse(data);
                    OnDelete?.Invoke(this, new StreamDeleteEventArgs() { StatusId = statusId });
                    break;
                case "filters_changed":
                    OnFiltersChanged?.Invoke(this, new StreamFiltersChangedEventArgs());
                    break;
                case "conversation":
                    var conversation = JsonConvert.DeserializeObject<Conversation>(data);
                    OnConversation?.Invoke(this,
                        new StreamConversationEvenTargs() { Conversation = conversation });
                    break;
            }
        }

        public void Stop()
        {
            // connected with http
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

#if NETSTANDARD2_0
            if (socket != null)
            {
                socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                socket.Dispose();
                socket = null;
            }
#endif
        }
    }
}