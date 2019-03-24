using Mastonet.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            await StartHttp();
//#if NETSTANDARD2_0
//            await StartWebSocket();
//#else
//            await StartHttp();
//#endif
        }

        private async Task StartWebSocket()
        {
            var instance = await instanceGetter;
            //TODO : is websocket supported ?
            var streamApiUrl = instance.Urls.StreamingAPI;

        }

        private async Task StartHttp()
        {
            var instance = await instanceGetter;
            string url = "https://" + instance.Uri;
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
                    url += "/api/v1/streaming/hashtag?tag="+param;
                    break;
                case StreamingType.HashtagLocal:
                    url += "/api/v1/streaming/hashtag/local?tag="+param;
                    break;
                case StreamingType.List:
                    url += "/api/v1/streaming/list?list="+param;
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
            }
            this.Stop();
        }

        public void Stop()
        {
            //if (client != null)
            //{
            //    client.Dispose();
            //    client = null;
            //}
        }
    }
}