using System;
using System.Threading.Tasks;
using Nestodon.Entities;

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


        private async Task SendRequest(string route, string method = "GET")
        {
            string url = "https://"+ this.Instance + route;

            var a = new Account();
            
        }


        


    }
}
