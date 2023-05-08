using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Mastonet.Entities;

/// <summary>
/// Represents a custom emoji.
/// </summary>
public class Emoji
{
    /// <summary>
    /// The name of the custom emoji.
    /// </summary>
    [JsonProperty("shortcode")]
    public string Shortcode { get; set; } = string.Empty;

    /// <summary>
    /// A link to the custom emoji.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// A link to a static copy of the custom emoji.
    /// </summary>
    [JsonProperty("static_url")]
    public string StaticUrl { get; set; } = string.Empty;

    /// <summary>
    /// Whether this Emoji should be visible in the picker or unlisted.
    /// </summary>
    [JsonProperty("visible_in_picker")]
    public bool VisibleInPicker { get; set; }

    /// <summary>
    /// Used for sorting custom emoji in the picker.
    /// </summary>
    [JsonProperty("category")]
    public string? Category { get; set; }
}
