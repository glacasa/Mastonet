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
        private readonly HttpClient client;
        public string Instance { get; protected set; }
        public AppRegistration AppRegistration { get; set; }
        public Auth AuthToken { get; set; }

        protected BaseHttpClient(HttpClient client)
        {
            this.client = client;
        }

        #region Http helpers

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string route, IEnumerable<KeyValuePair<string, string>> query = null, IEnumerable<KeyValuePair<string, string>> form = null, IEnumerable<MediaDefinition> media = null)
        {
            // Build URL
            var urlBuilder = new StringBuilder("https://").Append(Instance).Append(route);
            if (query?.Any() == true)
            {
                urlBuilder.Append("?").Append(string.Join("&", query.Select(kvp => kvp.Key + "=" + kvp.Value)));
            }
            var url = urlBuilder.ToString();

            // Build request
            using (var request = new HttpRequestMessage(method, url))
            {
                if (media?.Any() == true)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var m in media)
                    {
                        content.Add(new StreamContent(m.Media), m.ParamName, m.FileName);
                    }
                    if (form?.Any() == true)
                    {
                        foreach (var pair in form)
                        {
                            content.Add(new StringContent(pair.Value), pair.Key);
                        }
                    }
                    request.Content = content;
                }
                else if (form?.Any() == true)
                {
                    request.Content = new FormUrlEncodedContent(form);
                }
                request.Headers.Add("Authorization", "Bearer " + AuthToken.AccessToken);

                // And send it
                return await client.SendAsync(request);
            }
        }

        protected async Task<string> Delete(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            using (var response = await SendAsync(HttpMethod.Delete, route, query: data))
                return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> Get(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            using (var response = await SendAsync(HttpMethod.Get, route, query: data))
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
            using (var response = await SendAsync(HttpMethod.Get, route, query: data))
            {
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
        }

        protected async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            using (var response = await SendAsync(HttpMethod.Post, route, form: data))
                return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> PostMedia(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
        {
            using (var response = await SendAsync(HttpMethod.Post, route, form: data, media: media))
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
            using (var response = await SendAsync(HttpMethod.Put, route, form: data))
                return await response.Content.ReadAsStringAsync();
        }

        protected async Task<T> Put<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            return TryDeserialize<T>(await Put(route, data));
        }

        protected async Task<string> Patch(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            using (var response = await SendAsync(new HttpMethod("PATCH"), route, form: data))
                return await response.Content.ReadAsStringAsync();
        }

        protected async Task<string> PatchMedia(string route, IEnumerable<KeyValuePair<string, string>> data = null, IEnumerable<MediaDefinition> media = null)
        {
            using (var response = await SendAsync(new HttpMethod("PATCH"), route, form: data, media: media))
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
