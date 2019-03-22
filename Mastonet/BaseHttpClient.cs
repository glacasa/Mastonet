using Mastonet.Entities;
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
        public string Instance { get; protected set; }
        public AppRegistration AppRegistration { get; set; }
        public Auth AuthToken { get; set; }

        #region Http helpers

        private void AddHttpHeader(HttpClient client)
        {
            if (AuthToken != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthToken.AccessToken);
            }
        }

        protected async Task<string> Delete(string route)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);
            var response = await client.DeleteAsync(url);
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

            var client = new HttpClient();
            AddHttpHeader(client);
            var response = await client.GetAsync(url);
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

            var client = new HttpClient();
            AddHttpHeader(client);
            var response = await client.GetAsync(url);
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

            var client = new HttpClient();
            AddHttpHeader(client);

            var content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> PostMedia(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);

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

            var response = await client.PostAsync(url, content);
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

            var client = new HttpClient();
            AddHttpHeader(client);

            var content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var response = await client.PutAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Put<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            return TryDeserialize<T>(await Put(route, data));
        }

        protected async Task<string> Patch(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            var method = new HttpMethod("PATCH");
            AddHttpHeader(client);

            var content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var message = new HttpRequestMessage(method, url);
            message.Content = content;
            var response = await client.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> PatchMedia(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            var method = new HttpMethod("PATCH");
            AddHttpHeader(client);

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

            var message = new HttpRequestMessage(method, url);
            message.Content = content;
            var response = await client.SendAsync(message);
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
