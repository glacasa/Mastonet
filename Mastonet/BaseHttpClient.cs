﻿using Mastonet.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mastonet
{
    public abstract class BaseHttpClient : IBaseHttpClient
    {
        private readonly HttpClient client;
        public string Instance { get; protected set; }
        public AppRegistration AppRegistration { get; set; }
        public Auth AuthToken { get; set; }

        protected BaseHttpClient(HttpClient client)
        {
            this.client = client;
        }

        #region Http helpers

        private void AddHttpHeader(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", "Bearer " + AuthToken.AccessToken);
        }

        protected async Task<string> Delete(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;
            if (data != null)
            {
                var querystring = "?" + String.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
                url += querystring;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            AddHttpHeader(request);
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> Get(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;
            if (data != null)
            {
                var querystring = "?" + String.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
                url += querystring;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddHttpHeader(request);
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Get<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
            where T : class
        {
            var content = await Get(route, data);
            return TryDeserialize<T>(content);
        }

        private Regex idFinderRegex = new Regex("_id=([0-9]+)");
        protected async Task<MastodonList<T>> GetMastodonList<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;
            if (data != null)
            {
                var querystring = "?" + String.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
                url += querystring;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddHttpHeader(request);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var result = TryDeserialize<MastodonList<T>>(content);

            // Read `Link` header
            IEnumerable<string> linkHeader;
            if (response.Headers.TryGetValues("Link", out linkHeader))
            {
                var links = linkHeader.Single().Split(',');
                foreach (var link in links)
                {
                    if (link.Contains("rel=\"next\""))
                    {
                        result.NextPageMaxId = long.Parse(idFinderRegex.Match(link).Groups[1].Value);
                    }

                    if (link.Contains("rel=\"prev\""))
                    {
                        result.PreviousPageSinceId = long.Parse(idFinderRegex.Match(link).Groups[1].Value);
                    }
                }
            }

            return result;
        }

        protected async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddHttpHeader(request);
            request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> PostMedia(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
        {
            string url = "https://" + this.Instance + route;

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddHttpHeader(request);
            var content = new MultipartFormDataContent();

            foreach (var m in media)
            {
                content.Add(new StreamContent(m.Media), m.ParamName, m.FileName);
            }
            if (data != null)
            {
                foreach (var pair in data)
                {
                    content.Add(new StringContent(pair.Value), pair.Key);
                }
            }
            request.Content = content;

            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Post<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
            where T : class
        {
            var content = media != null && media.Any() ? await PostMedia(route, data, media) : await Post(route, data);
            return TryDeserialize<T>(content);
        }

        protected async Task<string> Put(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            AddHttpHeader(request);

            request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Put<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            return TryDeserialize<T>(await Put(route, data));
        }

        protected async Task<string> Patch(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            AddHttpHeader(request);
            request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> PatchMedia(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
        {
            string url = "https://" + this.Instance + route;

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            AddHttpHeader(request);

            var content = new MultipartFormDataContent();
            foreach (var m in media)
            {
                content.Add(new StreamContent(m.Media), m.ParamName, m.FileName);
            }
            if (data != null)
            {
                foreach (var pair in data)
                {
                    content.Add(new StringContent(pair.Value), pair.Key);
                }
            }

            request.Content = content;
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Patch<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
            where T : class
        {
            var content = media != null && media.Any() ? await PatchMedia(route, data, media) : await Patch(route, data);
            return TryDeserialize<T>(content);
        }

        private T TryDeserialize<T>(string json)
        {
            if (json[0] == '{')
            {
                var error = JsonConvert.DeserializeObject<Error>(json);
                if (!string.IsNullOrEmpty(error.Description))
                {
                    throw new ServerErrorException(error);
                }
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion
    }
}
