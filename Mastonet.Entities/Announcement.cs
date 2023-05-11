using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents an announcement set by an administrator.
/// </summary>
public class Announcement
{
    // Base attributes 

    /// <summary>
    /// The announcement id.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The content of the announcement.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Whether the announcement is currently active.
    /// </summary>
    [JsonPropertyName("published")]
    public bool Published { get; set; }

    /// <summary>
    /// Whether the announcement has a start/end time.
    /// </summary>
    [JsonPropertyName("all_day")]
    public bool AllDay { get; set; }

    /// <summary>
    /// When the announcement was created.
    /// </summary>
    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    /// <summary>
    /// When the announcement was last updated.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Whether the announcement has been read by the user.
    /// </summary>
    [JsonPropertyName("read")]
    public bool Read { get; set; }

    /// <summary>
    /// Emoji reactions attached to the announcement.
    /// </summary>
    [JsonPropertyName("reactions")]
    public IEnumerable<AnnouncementReaction> Reactions { get; set; } = Enumerable.Empty<AnnouncementReaction>();

    // Optional attributes

    /// <summary>
    /// When the future announcement was scheduled.
    /// </summary>
    [JsonPropertyName("scheduled_at")]
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// When the future announcement will start.
    /// </summary>
    [JsonPropertyName("starts_at")]
    public DateTime? StartsAt { get; set; }

    /// <summary>
    /// When the future announcement will end.
    /// </summary>
    [JsonPropertyName("ends_at")]
    public DateTime? EndsAt { get; set; }
}

/// <summary>
/// Represents an emoji reaction to an Announcement.
/// </summary>
/// <see href="https://docs.joinmastodon.org/entities/announcementreaction/"/>
public class AnnouncementReaction
{
    // Base attributes

    /// <summary>
    /// The emoji used for the reaction. Either a unicode emoji, or a custom emoji's shortcode.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The total number of users who have added this reaction.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }

    /// <summary>
    /// Whether the authorized user has added this reaction to the announcement.
    /// </summary>
    [JsonPropertyName("me")]
    public bool Me { get; set; }

    // Custom emoji attributes

    /// <summary>
    /// A link to the custom emoji.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// A link to a non-animated version of the custom emoji.
    /// </summary>
    [JsonPropertyName("static_url")]
    public string StaticUrl { get; set; } = string.Empty;
}
