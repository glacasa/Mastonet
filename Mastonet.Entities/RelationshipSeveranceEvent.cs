using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities
{
    public  class RelationshipSeveranceEvent
    {
        /// <summary>
        /// The ID of the relationship severance event in the database.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Type of event.One of:
        /// domain_block = A moderator suspended a whole domain
        /// user_domain_block = The user blocked a whole domain
        /// account_suspension = A moderator suspended a specific account
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Whether the list of severed relationships is unavailable because the underlying issue has been purged.
        /// </summary>
        [JsonPropertyName("purged")]
        public bool Purged { get; set; }

        /// <summary>
        /// Name of the target of the moderation/block event. This is either a domain name or a user handle, depending on the event type.
        /// </summary>
        [JsonPropertyName("target_name")]
        public string TargetName { get; set; } = string.Empty;

        /// <summary>
        /// Number of follow relationships (in either direction) that were severed.
        /// </summary>
        [JsonPropertyName("relationships_count")]
        public int RelationshipsCount { get; set; }

        /// <summary>
        /// When the event took place.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string CrreatedAt { get; set; } = string.Empty;
    }
}
