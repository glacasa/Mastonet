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
    }
}
