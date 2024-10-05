using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities
{
    public class NotificationRequest
    {
        /// <summary>
        /// The id of the notification request in the database.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The timestamp of the notification request, i.e. when the first filtered notification from that user was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        /// <summary>
        /// The timestamp of when the notification request was last updated.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = string.Empty;

        /// <summary>
        /// The account that performed the action that generated the filtered notifications.
        /// </summary>
        [JsonPropertyName("account")]
        public Account Account { get; set; } = new Account();

        /// <summary>
        /// How many of this account’s notifications were filtered.
        /// </summary>
        [JsonPropertyName("notifications_count")]
        public string NotificationsCount { get; set; } = string.Empty;

        /// <summary>
        /// Most recent status associated with a filtered notification from that account.
        /// </summary>
        [JsonPropertyName("last_status")]
        public string LastStatus { get; set; } = string.Empty;
    }
}
