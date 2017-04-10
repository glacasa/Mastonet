using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Relationship
    {
        /// <summary>
        /// Whether the user is currently following the account
        /// </summary>
        [JsonProperty("following")]
        public bool Following { get; set; }

        /// <summary>
        /// Whether the user is currently being followed by the account
        /// </summary>
        [JsonProperty("followed_by")]
        public bool FollowedBy { get; set; }

        /// <summary>
        /// Whether the user is currently blocking the account
        /// </summary>
        [JsonProperty("blocking")]
        public bool Blocking { get; set; }

        /// <summary>
        /// Whether the user is currently muting the account
        /// </summary>
        [JsonProperty("muting")]
        public bool Muting { get; set; }

        /// <summary>
        /// Whether the user has requested to follow the account
        /// </summary>
        [JsonProperty("requested")]
        public bool Requested { get; set; }
    }
}
