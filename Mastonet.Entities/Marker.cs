using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents the last read position within a user's timelines.
/// </summary>
public class Marker
{
    /// <summary>
    /// Information about the user's position in the home timeline.
    /// </summary>
    [JsonPropertyName("home")]
    public MarkerInfo Home { get; set; } = default!;

    /// <summary>
    /// Information about the user's position in their notifications.
    /// </summary>
    [JsonPropertyName("notifications")]
    public MarkerInfo Notifications { get; set; } = default!;

}

public class MarkerInfo
{
    /// <summary>
    /// The ID of the most recently viewed entity.
    /// </summary>
    [JsonPropertyName("last_read_id")]
    public long LastReadId { get; set; }

    /// <summary>
    /// Used for locking to prevent write conflicts.
    /// </summary>
    [JsonPropertyName("version")]
    public int Version { get; set; }

    /// <summary>
    /// The timestamp of when the marker was set.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
