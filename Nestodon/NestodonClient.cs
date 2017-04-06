using System;
using System.Threading.Tasks;
using Nestodon.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Nestodon
{
    public class NestodonClient
    {
        public string Instance { get; }

        public string ClientId { get;  }

        public string ClientSecret { get;  }

        #region Ctor

        public NestodonClient(string instance)
        {
            this.Instance = instance;
        }

        public NestodonClient(string instance, string clientId, string clientSecret)
        {
            this.Instance = instance;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        #endregion

        #region Http helpers

        private static void AddHttpHeader(HttpClient client)
        {
            // TODO : add access token
            client.DefaultRequestHeaders.Add("Authorization", "Bearer ");
        }

        private async Task<string> Get(string route)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> GetObject<T>(string route)
        {
            var content = await Get(route);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        public async Task Connect(string email, string password)
        {
            var data = new List<KeyValuePair<string, string>>();
            data.Add(new KeyValuePair<string, string>("client_id", ClientId));
            data.Add(new KeyValuePair<string, string>("client_secret", ClientSecret));
            data.Add(new KeyValuePair<string, string>("grant_type", "password"));
            data.Add(new KeyValuePair<string, string>("username", email));
            data.Add(new KeyValuePair<string, string>("password", password));

            var response = await Post("/oauth/token", data);

        }

        public async Task RegisterApp(string appName)
        {
            var data = new List<KeyValuePair<string, string>>();
            data.Add(new KeyValuePair<string, string>("client_name", appName));
            data.Add(new KeyValuePair<string, string>("redirect_uris", "urn:ietf:wg:oauth:2.0:oob"));
            data.Add(new KeyValuePair<string, string>("scopes", "read"));

            var response = await Post("/api/v1/apps", data);
        }


        public Task<Account> GetAccount(string name)
        {
            return GetObject<Account>($"/api/v1/accounts/{name}");
        }



    }
}
