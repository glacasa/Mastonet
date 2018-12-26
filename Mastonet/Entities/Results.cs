using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#results
    public class Results
    {
        /// <summary>
        /// An array of matched Accounts
        /// </summary>
        [JsonProperty("accounts")]
        public IEnumerable<Account> Accounts { get; set; } = new Account[0];

        /// <summary>
        /// An array of matchhed Statuses
        /// </summary>
        [JsonProperty("statuses")]
        public IEnumerable<Status> Statuses { get; set; } = new Status[0];

        /// <summary>
        /// An array of matched hashtags, as strings
        /// </summary>
        [JsonProperty("hashtags")]
        public IEnumerable<string> Hashtags { get; set; } = new string[0];
    }
}
