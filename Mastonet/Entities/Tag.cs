using Newtonsoft.Json;
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
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A link to the hashtag on the instance.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Usage statistics for given days.
    /// </summary>
    [JsonProperty("history")]
    public IEnumerable<History>? History { get; set; }
    
    [JsonProperty("following")]
    public bool? Following { get; set; }
}
