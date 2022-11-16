using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a profile field as a name-value pair with optional verification.
/// </summary>
public class Field
{
    /// <summary>
    /// The key of a given field's key-value pair.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The value associated with the name key.
    /// </summary>
    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of when the server verified a URL value for a rel="me” link.
    /// </summary>
    [JsonProperty("verified_at")]
    public DateTime? VerifiedAt { get; set; }
}
