using Newtonsoft.Json;
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
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The website associated with your application.
    /// </summary>
    [JsonProperty("website")]
    public string? Website { get; set; }

    /// <summary>
    /// Used for Push Streaming API.
    /// </summary>
    [JsonProperty("vapid_key")]
    public string? VapidKey { get; set; }
}
