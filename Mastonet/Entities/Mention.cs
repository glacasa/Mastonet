using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#mention
    public class Mention
    {

        /// <summary>
        /// URL of user's profile (can be remote)
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// The username of the account
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Equals username for local users, includes @domain for remote ones
        /// </summary>
        [JsonProperty("acct")]
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Account ID
        /// </summary>
        [JsonProperty("id")]
        public long AccountId { get; set; }

        [JsonIgnore]
        [Obsolete("Id has been renamed to AccountId to reflec actual usage")]
        public long Id
        {
            get { return AccountId; }
            set { AccountId = value; }
        }
    }
}
