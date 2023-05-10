using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents an application that interfaces with the REST API to access accounts or post statuses.
/// </summary>
public class Application
{
    /// <summary>
    /// The name of your application.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The website associated with your application.
    /// </summary>
    [JsonPropertyName("website")]
    public string? Website { get; set; }

    /// <summary>
    /// Used for Push Streaming API.
    /// </summary>
    [JsonPropertyName("vapid_key")]
    public string? VapidKey { get; set; }
}
