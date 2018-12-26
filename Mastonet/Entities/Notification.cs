using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#notification
    public class Notification
    {
        /// <summary>
        /// The notification ID
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// One of: "mention", "reblog", "favourite", "follow"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The time the notification was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The Account sending the notification to the user
        /// </summary>
        [JsonProperty("account")]
        public Account Account { get; set; } = new Account();

        /// <summary>
        /// The Status associated with the notification, if applicible
        /// </summary>
        [JsonProperty("status")]
        public Status? Status { get; set; }
    }
}
