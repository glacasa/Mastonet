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
        private readonly string instance;
        private readonly string path;

        private readonly string accessToken;
        private HttpClient client;


        public event EventHandler<StreamUpdateEventArgs> OnUpdate;
        public event EventHandler<StreamNotificationEventArgs> OnNotification;
        public event EventHandler<StreamDeleteEventArgs> OnDelete;
        public event EventHandler<StreamFiltersChangedEventArgs> OnFiltersChanged;
        public event EventHandler<StreamConversationEvenTargs> OnConversation;

        internal TimelineStreaming(string instance, string path, string accessToken)
        {
            this.instance = instance;
            this.path = path;
            this.accessToken = accessToken;
        }

        public async Task Start()
        {
            //TODO : get Streaming url from GetInstance() ; use websocket instead of http server-sent event
            var url = "https://" + instance + this.path;
            await StartHttp(url);
        }

        private async Task StartHttp(string url)
        {
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

        private Task StartWebSocket(string url)
        {
            // TODO : use web sockets
            throw new NotImplementedException();
        }

        public void Stop()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}