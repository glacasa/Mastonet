using Mastonet.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mastonet;

public abstract class BaseHttpClient
{
    protected readonly HttpClient client;

    public string AccessToken { get; protected set; } = string.Empty;

    #region Instance 
    private string instance = string.Empty;
    public string Instance
    {
        get
        {
            return instance;
        }
        protected set
        {
            instance = CheckInstance(value);
        }
    }

    private string CheckInstance(string instance)
    {
        if (string.IsNullOrWhiteSpace(instance))
        {
            throw new ArgumentNullException(nameof(instance));
        }

        if (instance.StartsWith("https://"))
        {
            instance = instance.Substring("https://".Length);
        }

        var notSupportedList = new List<string> { "gab.", "truthsocial." };
        var lowered = instance.ToLowerInvariant();
        if (notSupportedList.Any(n => lowered.Contains(n)))
        {
            throw new NotSupportedException();
        }

        return instance;
    }

    #endregion

    protected BaseHttpClient(HttpClient client)
    {
        this.client = client;
    }

    #region Http helpers

    protected abstract void OnResponseReceived(HttpResponseMessage response);
    
    private void AddHttpHeader(HttpRequestMessage request)
    {
        if (!string.IsNullOrEmpty(AccessToken))
        {
            request.Headers.Add("Authorization", "Bearer " + AccessToken);
        }
    }

    protected async Task<string> Delete(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        string url = "https://" + this.Instance + route;
        if (data != null)
        {
            var querystring = "?" + String.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
            url += querystring;
        }

        using (var request = new HttpRequestMessage(HttpMethod.Delete, url))
        {
            AddHttpHeader(request);
            using var response = await client.SendAsync(request);
            OnResponseReceived(response);
            return await response.Content.ReadAsStringAsync();
        }
    }


    protected async Task<string> Get(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        string url = "https://" + this.Instance + route;
        if (data != null)
        {
            var querystring = "?" + String.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
            url += querystring;
        }

        using (var request = new HttpRequestMessage(HttpMethod.Get, url))
        {
            AddHttpHeader(request);
            using var response = await client.SendAsync(request);
            OnResponseReceived(response);
            return await response.Content.ReadAsStringAsync();
        }
    }

    protected async Task<T> Get<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
        where T : class
    {
        var content = await Get(route, data);
        return TryDeserialize<T>(content);
    }

    private Regex idFinderRegex = new Regex("_id=([0-9]+)");
    protected async Task<MastodonList<T>> GetMastodonList<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        string url = "https://" + this.Instance + route;
        if (data != null)
        {
            var querystring = "?" + String.Join("&", data.Select(kvp => kvp.Key + "=" + kvp.Value));
            url += querystring;
        }

        using (var request = new HttpRequestMessage(HttpMethod.Get, url))
        {
            AddHttpHeader(request);
            using (var response = await client.SendAsync(request))
            {
                OnResponseReceived(response);
                var content = await response.Content.ReadAsStringAsync();
                var result = TryDeserialize<MastodonList<T>>(content);

                // Read `Link` header
                IEnumerable<string>? linkHeader;
                if (response.Headers.TryGetValues("Link", out linkHeader))
                {
                    var links = linkHeader.Single().Split(',');
                    foreach (var link in links)
                    {
                        if (link.Contains("rel=\"next\""))
                        {
                            result.NextPageMaxId = idFinderRegex.Match(link).Groups[1].Value;
                        }

                        if (link.Contains("rel=\"prev\""))
                        {
                            if (link.Contains("since_id"))
                            {
                                result.PreviousPageSinceId = idFinderRegex.Match(link).Groups[1].Value;
                            }
                            if (link.Contains("min_id"))
                            {
                                result.PreviousPageMinId = idFinderRegex.Match(link).Groups[1].Value;
                            }
                        }
                    }
                }

                return result;
            }
        }
    }

    protected async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        string url = "https://" + this.Instance + route;

        using (var request = new HttpRequestMessage(HttpMethod.Post, url))
        {
            AddHttpHeader(request);
            request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            using var response = await client.SendAsync(request);
            OnResponseReceived(response);
            return await response.Content.ReadAsStringAsync();
        }
    }

    protected async Task<string> PostMedia(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
    {
        string url = "https://" + this.Instance + route;

        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        AddHttpHeader(request);
        var content = new MultipartFormDataContent();

        if (media != null)
        {
            foreach (var m in media)
            {
                content.Add(new StreamContent(m.Media), m.ParamName!, m.FileName);
            }
        }

        if (data != null)
        {
            foreach (var pair in data)
            {
                content.Add(new StringContent(pair.Value), pair.Key);
            }
        }
        request.Content = content;

        using var response = await client.SendAsync(request);
        OnResponseReceived(response);
        return await response.Content.ReadAsStringAsync();
    }

    protected async Task<T> Post<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
        where T : class
    {
        var content = media != null && media.Any() ? await PostMedia(route, data, media) : await Post(route, data);
        return TryDeserialize<T>(content);
    }

    protected async Task<string> Put(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        string url = "https://" + this.Instance + route;

        using (var request = new HttpRequestMessage(HttpMethod.Put, url))
        {
            AddHttpHeader(request);

            request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            using var response = await client.SendAsync(request);
            OnResponseReceived(response);
            return await response.Content.ReadAsStringAsync();
        }
    }

    protected async Task<T> Put<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        return TryDeserialize<T>(await Put(route, data));
    }

    protected async Task<string> Patch(string route, IEnumerable<KeyValuePair<string, string>>? data = null)
    {
        string url = "https://" + this.Instance + route;

        using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), url))
        {
            AddHttpHeader(request);
            request.Content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            using var response = await client.SendAsync(request);
            OnResponseReceived(response);
            return await response.Content.ReadAsStringAsync();
        }
    }

    protected async Task<string> PatchMedia(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
    {
        string url = "https://" + this.Instance + route;

        using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), url))
        {

            AddHttpHeader(request);

            var content = new MultipartFormDataContent();

            if (media != null)
            {
                foreach (var m in media)
                {
                    content.Add(new StreamContent(m.Media), m.ParamName!, m.FileName);
                }
            }

            if (data != null)
            {
                foreach (var pair in data)
                {
                    content.Add(new StringContent(pair.Value), pair.Key);
                }
            }

            request.Content = content;
            using var response = await client.SendAsync(request);
            OnResponseReceived(response);
            return await response.Content.ReadAsStringAsync();
        }
    }

    protected async Task<T> Patch<T>(string route, IEnumerable<KeyValuePair<string, string>>? data = null, IEnumerable<MediaDefinition>? media = null)
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
            if (error != null && !string.IsNullOrEmpty(error.Description))
            {
                throw new ServerErrorException(error);
            }
        }

        return JsonConvert.DeserializeObject<T>(json)!;
    }

    #endregion


}
