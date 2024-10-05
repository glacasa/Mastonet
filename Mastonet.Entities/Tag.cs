using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a hashtag used within the content of a status.
/// </summary>
public class Tag
{
    /// <summary>
    /// The value of the hashtag after the # sign.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A link to the hashtag on the instance.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Usage statistics for given days.
    /// </summary>
    [JsonPropertyName("history")]
    public IEnumerable<History>? History { get; set; }

    /// <summary>
    /// Whether the current token’s authorized user is following this tag.
    /// </summary>
    [JsonPropertyName("following")]
    public bool? Following { get; set; }
}
