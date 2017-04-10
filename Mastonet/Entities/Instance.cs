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
    }
}
