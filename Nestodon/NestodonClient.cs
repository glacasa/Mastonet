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

        public AppRegistration AppRegistration { get; set; }

        public Auth UserAuth { get; set; }


        #region Ctor

        public NestodonClient(string instance, AppRegistration appRegistration = null, Auth userAuth = null)
        {
            this.Instance = instance;
            this.AppRegistration = appRegistration;
            this.UserAuth = userAuth;
        }

        #endregion

        #region Http helpers

        private void AddHttpHeader(HttpClient client)
        {
            if (UserAuth != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + UserAuth.AccessToken);
            }
        }

        private async Task<string> Get(string route)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);
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
            AddHttpHeader(client);
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> Post<T>(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            var content = await Post(route, data);
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion

        public async Task<Auth> Connect(string email, string password)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
            };



            this.UserAuth = await Post<Auth>("/oauth/token", data);
            return this.UserAuth;
        }

        public async Task<AppRegistration> RegisterApp(string appName)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("client_name", appName),
                new KeyValuePair<string, string>("redirect_uris", "urn:ietf:wg:oauth:2.0:oob"),
                new KeyValuePair<string, string>("scopes", "read"),
            };

            this.AppRegistration = await Post<AppRegistration>("/api/v1/apps", data);

            return this.AppRegistration;
        }


        public Task<Account> GetAccount(int id)
        {
            return GetObject<Account>($"/api/v1/accounts/{id}");
        }



    }
}
