using System;
using System.Threading.Tasks;

namespace Nestodon
{
    public class NestodonClient
    {
        /// <summary>
        /// Create a new client
        /// </summary>
        public NestodonClient()
        {
        }


        private async Task SendRequest(string route, string method = "GET")
        {
            await Task.Delay(100);
        }

        


    }
}
