using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Instance
    {
        /// <summary>
        /// URI of the current instance
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// The instance's title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// A description for the instance
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// An email address which can be used to contact the instance administrator
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The Mastodon version of the instance
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// URI for the thumbnail of the hero image
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// URLs related to the instance
        /// </summary>
        [JsonProperty("urls")]
        public InstanceUrls Urls { get; set; }

        /// <summary>
        /// The instance's stats
        /// </summary>
        [JsonProperty("stats")]
        public InstanceStats Stats { get; set; }

        /// <summary>
        /// Array that consists of the instance's default locale
        /// </summary>
        [JsonProperty("languages")]
        public IEnumerable<string> Languages { get; set; }

        /// <summary>
        /// The instance's admin account
        /// </summary>
        [JsonProperty("contact_account")]
        public Account ContactAccount { get; set; }
    }

    public class InstanceUrls
    {
        /// <summary>
        /// Websocket base URL for streaming API
        /// </summary>
        [JsonProperty("streaming_api")]
        public string StreamingAPI { get; set; }
    }

    public class InstanceStats
    {
        /// <summary>
        /// Number of users that belongs to the instance
        /// </summary>
        [JsonProperty("user_count")]
        public int UserCount { get; set; }

        /// <summary>
        /// Number of statuses that belongs to the instance
        /// </summary>
        [JsonProperty("status_count")]
        public int StatusCount { get; set; }

        /// <summary>
        /// Number of remote instances known to the instance
        /// </summary>
        [JsonProperty("domain_count")]
        public int DomainCount { get; set; }
    }
}
