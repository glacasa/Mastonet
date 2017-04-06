using System;
using System.Threading.Tasks;
using Nestodon.Entities;
using System.Net.Http;
using Newtonsoft.Json;

namespace Nestodon
{
    public class NestodonClient
    {
        public string Instance { get; }

        /// <summary>
        /// Create a new client
        /// </summary>
        public NestodonClient(string instance)
        {
            this.Instance = instance;
        }


        private async Task<string> SendRequest(string route, string method = "GET")
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> GetObject<T>(string route, string method = "GET")
        {
            var content = await SendRequest(route, method);
            return JsonConvert.DeserializeObject<T>(content);
        }


        public Task<Account> GetAccount(string name)
        {
            return GetObject<Account>($"/api/v1/accounts/{name}");
        }



    }
}
