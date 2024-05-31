using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a hashtag that is featured on a profile.
/// </summary>
public class FeaturedTag
{
    /// <summary>
    /// The internal ID of the featured tag in the database.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The name of the hashtag being featured.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A link to all statuses by a user that contain this hashtag.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }= string.Empty;

    /// <summary>
    /// The number of authored statuses containing this hashtag.
    /// </summary>
    [JsonPropertyName("statuses_count")]
    public string StatusesCount { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp of the last authored status containing this hashtag.
    /// </summary>
    [JsonPropertyName("last_status_at")]
    public DateTime? LastStatusAt { get; set; }
}
