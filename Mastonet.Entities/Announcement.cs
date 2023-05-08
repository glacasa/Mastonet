using Newtonsoft.Json;
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
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The content of the announcement.
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;
    
    [Obsolete("This property was incorrectly named, use 'Content' instead.")]
    public string Text
    {
        get => Content;
        set => Content = value;
    }

    /// <summary>
    /// Whether the announcement is currently active.
    /// </summary>
    [JsonProperty("published")]
    public bool Published { get; set; }

    /// <summary>
    /// Whether the announcement has a start/end time.
    /// </summary>
    [JsonProperty("all_day")]
    public bool AllDay { get; set; }

    /// <summary>
    /// When the announcement was created.
    /// </summary>
    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [Obsolete("This property was incorrectly named, use 'PublishedAt' instead.")]
    public DateTime CreatedAt    {
        get => PublishedAt;
        set => PublishedAt = value;
    }

    /// <summary>
    /// When the announcement was last updated.
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Whether the announcement has been read by the user.
    /// </summary>
    [JsonProperty("read")]
    public bool Read { get; set; }

    /// <summary>
    /// Emoji reactions attached to the announcement.
    /// </summary>
    [JsonProperty("reactions")]
    public IEnumerable<AnnouncementReaction> Reactions { get; set; } = Enumerable.Empty<AnnouncementReaction>();

    // Optional attributes

    /// <summary>
    /// When the future announcement was scheduled.
    /// </summary>
    [JsonProperty("scheduled_at")]
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// When the future announcement will start.
    /// </summary>
    [JsonProperty("starts_at")]
    public DateTime? StartsAt { get; set; }

    /// <summary>
    /// When the future announcement will end.
    /// </summary>
    [JsonProperty("ends_at")]
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
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The total number of users who have added this reaction.
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; set; }

    /// <summary>
    /// Whether the authorized user has added this reaction to the announcement.
    /// </summary>
    [JsonProperty("me")]
    public bool Me { get; set; }

    // Custom emoji attributes

    /// <summary>
    /// A link to the custom emoji.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// A link to a non-animated version of the custom emoji.
    /// </summary>
    [JsonProperty("static_url")]
    public string StaticUrl { get; set; } = string.Empty;
}
