using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities
{
    public class AccountWarning
    {
        /// <summary>
        /// The ID of the account warning in the database.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Action taken against the account.
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Message from the moderator to the target account.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// List of status IDs that are relevant to the warning. When action is mark_statuses_as_sensitive or delete_statuses, those are the affected statuses.
        /// </summary>
        [JsonPropertyName("status_ids")]
        public IEnumerable<string> StatusIds { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Account against which a moderation decision has been taken.
        /// </summary>
        [JsonPropertyName("target_account")]
        public Account TargetAccount { get; set; } = new Account();

        /// <summary>
        /// Appeal submitted by the target account, if any.
        /// </summary>
        [JsonPropertyName("appeal")]
        public Appeal? Appeal { get; set; }

        /// <summary>
        /// When the event took place.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;
    }
}
