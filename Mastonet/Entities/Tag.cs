using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Tag
    {
        /// <summary>
        /// The hashtag, not including the preceding #
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The URL of the hashtag
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 7-day stats of the hashtag
        /// </summary>
        [JsonProperty("history")]
        public IEnumerable<History> History { get; set; }
    }

    public class History
    {
        /// <summary>
        /// UNIX time of the beginning of the day
        /// </summary>
        [JsonProperty("day")]
        public int Day { get; set; }

        /// <summary>
        /// Number of statuses with the hashtag during the day
        /// </summary>
        [JsonProperty("uses")]
        public int Uses { get; set; }

        /// <summary>
        /// Number of accounts that used the hashtag during the day
        /// </summary>
        [JsonProperty("accounts")]
        public int Accounts { get; set; }
    }
}
