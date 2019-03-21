using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class ScheduledStatus
    {
        /// <summary>
        /// The scheduled status's ID
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// DateTime to publish the scheduled status
        /// </summary>
        [JsonProperty("scheduled_at")]
        public DateTime ScheduledAt { get; set; }

        /// <summary>
        /// Parameters of the scheduled status
        /// </summary>
        [JsonProperty("params")]
        public StatusParams Params { get; set; }

        /// <summary>
        /// Media attached to the scheduled status
        /// </summary>
        [JsonProperty("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; }
    }

    public class StatusParams
    {
        /// <summary>
        /// Content of the status in plain text
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// null or the ID of the status it replies to
        /// </summary>
        [JsonProperty("in_reply_to_id")]
        public string InReplyToId { get; set; }

        /// <summary>
        /// IDs of the attachments
        /// </summary>
        [JsonProperty("media_ids")]
        public IEnumerable<long> MediaIds { get; set; }

        /// <summary>
        /// Whether to mark the attachment as sensitive, or null 
        /// </summary>
        [JsonProperty("sensitive")]
        public bool? Sensitive { get; set; }

        /// <summary>
        /// Spoiler text if any
        /// </summary>
        [JsonProperty("spoiler_text")]
        public string SpoilerText { get; set; }

        /// <summary>
        /// Visibility of the scheduled status
        /// </summary>
        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }

        /// <summary>
        /// DateTime to publish the scheduled status
        /// </summary>
        [JsonProperty("scheduled_at")]
        public DateTime? ScheduledAt { get; set; }

        /// <summary>
        /// Application ID that created the scheduled status
        /// </summary>
        [JsonProperty("application_id")]
        public long ApplicationId { get; set; }
    }
}
