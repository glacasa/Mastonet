using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a status that will be published at a future scheduled date.
/// </summary>
public class ScheduledStatus
{
    /// <summary>
    /// ID of the scheduled status in the database.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// DateTime to publish the scheduled status
    /// </summary>
    [JsonPropertyName("scheduled_at")]
    public DateTime ScheduledAt { get; set; }

    /// <summary>
    /// Parameters of the scheduled status
    /// </summary>
    [JsonPropertyName("params")]
    public StatusParams Params { get; set; } = new StatusParams();

    /// <summary>
    /// Media attached to the scheduled status
    /// </summary>
    [JsonPropertyName("media_attachments")]
    public IEnumerable<Attachment> MediaAttachments { get; set; } = Enumerable.Empty<Attachment>();
}

public class StatusParams
{
    /// <summary>
    /// Content of the status in plain text
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// null or the ID of the status it replies to
    /// </summary>
    [JsonPropertyName("in_reply_to_id")]
    public string? InReplyToId { get; set; }

    /// <summary>
    /// IDs of the attachments
    /// </summary>
    [JsonPropertyName("media_ids")]
    public IEnumerable<long>? MediaIds { get; set; }

    /// <summary>
    /// Whether to mark the attachment as sensitive, or null 
    /// </summary>
    [JsonPropertyName("sensitive")]
    public bool? Sensitive { get; set; }

    /// <summary>
    /// Spoiler text if any
    /// </summary>
    [JsonPropertyName("spoiler_text")]
    public string? SpoilerText { get; set; }

    /// <summary>
    /// Visibility of the scheduled status
    /// </summary>
    [JsonPropertyName("visibility")]
    [JsonConverter(typeof(VisibilityConverter))]
    public Visibility Visibility { get; set; }

    /// <summary>
    /// DateTime to publish the scheduled status
    /// </summary>
    [JsonPropertyName("scheduled_at")]
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// Application ID that created the scheduled status
    /// </summary>
    [JsonPropertyName("application_id")]
    public long ApplicationId { get; set; }
}
