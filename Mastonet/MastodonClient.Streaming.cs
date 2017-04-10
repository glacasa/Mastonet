using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Mastonet.Entities;

namespace Mastonet
{
    public partial class MastodonClient
    {
        private StreamingMode mode = StreamingMode.None;

        public event EventHandler<StreamUpdateEventArgs> OnUpdate;
        public event EventHandler<StreamNotificationEventArgs> OnNotification;
        public event EventHandler<StreamDeleteEventArgs> OnDelete;

        public void StartPublicStreaming()
        {
            StopStreaming();
            mode = StreamingMode.Public;
            string url = "https://" + this.Instance + "/api/v1/streaming/public";
            StartStreaming(url);
        }



        public async void StartStreaming(string url)
        {            
            var client = new HttpClient();
            AddHttpHeader(client);

            var stream = await client.GetStreamAsync(url);

            var reader = new StreamReader(stream);

            string eventName = null;
            string data = null;

            while (mode != StreamingMode.None)
            {
                var line = reader.ReadLine();

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
                            var statusId = int.Parse(data);
                            OnDelete?.Invoke(this, new StreamDeleteEventArgs() { StatusId = statusId });
                            break;
                    }
                }
            }
        }

        public void StopStreaming()
        {
            mode = StreamingMode.None;
            // TODO : really stop
        }

    }

    public class StreamUpdateEventArgs : EventArgs
    {
        public Status Status { get; set; }
    }
    public class StreamNotificationEventArgs : EventArgs
    {
        public Notification Notification { get; set; }
    }
    public class StreamDeleteEventArgs : EventArgs
    {
        public int StatusId { get; set; }
    }


    internal enum StreamingMode
    {
        None,
        User,
        Public,
        Hashtag,
    }


}
