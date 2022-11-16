using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents an error message.
/// </summary>
public class Error
{
    /// <summary>
    /// The error message.
    /// </summary>
    [JsonProperty("error")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// A longer description of the error, mainly provided with the OAuth API.
    /// </summary>
    [JsonProperty("error_description")]
    public string? ErrorDescription { get; set; }
}
