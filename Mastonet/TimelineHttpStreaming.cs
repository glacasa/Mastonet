﻿using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mastonet
{
    public class TimelineHttpStreaming : TimelineStreaming
    {
        private string instance;
        private HttpClient client;

        public TimelineHttpStreaming(StreamingType type, string param, string instance, string accessToken)
            : base(type, param, accessToken)
        {
            this.instance = instance;
        }

        public override async Task Start(CancellationToken token)
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

            client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            var response = await client.SendAsync(request, token);
            var stream = await response.Content.ReadAsStreamAsync();
            var reader = new StreamReader(stream);

            string eventName = null;
            string data = null;

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
                    SendEvent(eventName, data);
                }
            }
        }
    }
}
