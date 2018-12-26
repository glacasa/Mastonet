using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#instance
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
        /// The Mastodon version used by instance.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// streaming_api
        /// </summary>
        [JsonProperty("urls")]
        public string Urls { get; set; } = string.Empty;

        /// <summary>
        /// Array of ISO 6391 language codes the instance has chosen to advertise
        /// </summary>
        [JsonProperty("languages")]
        public IEnumerable<string> Languages { get; set; } = new string[0];

        /// <summary>
        /// Account of the admin or another contact person
        /// </summary>
        [JsonProperty("contact_account")]
        public Account contact_account { get; set; } = new Account();
    }
}
