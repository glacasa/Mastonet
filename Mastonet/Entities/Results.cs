using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Results
    {
        /// <summary>
        /// An array of matched Accounts
        /// </summary>
        [JsonProperty("accounts")]
        public IEnumerable<Account> Accounts { get; set; }

        /// <summary>
        /// An array of matched Statuses
        /// </summary>
        [JsonProperty("statuses")]
        public IEnumerable<Status> Statuses { get; set; }

        /// <summary>
        /// An array of matched hashtags, as strings
        /// </summary>
        [JsonProperty("hashtags")]
        public IEnumerable<string> Hashtags { get; set; }
    }

    public class ResultsV2
    {
        /// <summary>
        /// An array of matched Accounts
        /// </summary>
        [JsonProperty("accounts")]
        public IEnumerable<Account> Accounts { get; set; }

        /// <summary>
        /// An array of matched Statuses
        /// </summary>
        [JsonProperty("statuses")]
        public IEnumerable<Status> Statuses { get; set; }

        /// <summary>
        /// An array of matched hashtags, as Tag instances
        /// </summary>
        [JsonProperty("hashtags")]
        public IEnumerable<Tag> Hashtags { get; set; }
    }
}
