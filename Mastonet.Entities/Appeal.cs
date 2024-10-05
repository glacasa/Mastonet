using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities
{
    public class Appeal
    {
        /// <summary>
        /// Text of the appeal from the moderated account to the moderators.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// State of the appeal. One of:
        /// approved = The appeal has been approved by a moderator
        /// rejected = The appeal has been rejected by a moderator
        /// pending = The appeal has been submitted, but neither approved nor rejected yet
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
    }
}
