using Newtonsoft.Json;
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
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The name of the hashtag being featured.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A link to all statuses by a user that contain this hashtag.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; }= string.Empty;

    /// <summary>
    /// The number of authored statuses containing this hashtag.
    /// </summary>
    [JsonProperty("statuses_count")]
    public long StatusesCount { get; set; }

    /// <summary>
    /// The timestamp of the last authored status containing this hashtag.
    /// </summary>
    [JsonProperty("last_status_at")]
    public DateTime LastStatusAt { get; set; }
}
