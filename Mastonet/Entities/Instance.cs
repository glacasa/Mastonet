using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities
{
    public class Instance
    {
        /// <summary>
        /// URI of the current instance
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// The instance's title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// A description for the instance
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// An email address which can be used to contact the instance administrator
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The Mastodon version of the instance
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// URI for the thumbnail of the hero image
        /// </summary>
        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }

        /// <summary>
        /// URLs related to the instance
        /// </summary>
        [JsonProperty("urls")]
        public InstanceUrls Urls { get; set; } = new InstanceUrls();

        /// <summary>
        /// The instance's stats
        /// </summary>
        [JsonProperty("stats")]
        public InstanceStats Stats { get; set; } = new InstanceStats();

        /// <summary>
        /// Array that consists of the instance's default locale
        /// </summary>
        [JsonProperty("languages")]
        public IEnumerable<string> Languages { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// The instance's admin account
        /// </summary>
        [JsonProperty("contact_account")]
        public Account? ContactAccount { get; set; }
    }

    public class InstanceUrls
    {
        /// <summary>
        /// Websocket base URL for streaming API
        /// </summary>
        [JsonProperty("streaming_api")]
        public string StreamingAPI { get; set; } = string.Empty;
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
