using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#tag
    public class Tag
    {
        /// <summary>
        /// The hashtag, not including the preceding #
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The URL of the hashtag
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Array of daily usage history. Not included in statuses
        /// </summary>
        [JsonProperty("history")]
        public IEnumerable<TagHistory>? History { get; set; }
    }

    public class TagHistory
    {
        public long Day { get; set; }

        public int Uses { get; set; }

        public int Accounts { get; set; }
    }
}
