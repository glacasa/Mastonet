using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Reports filed against users and/or statuses, to be taken action on by moderators.
/// </summary>
public class Report
{
    /// <summary>
    /// The ID of the report
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The action taken in response to the report
    /// </summary>
    [JsonPropertyName("action_taken")]
    public string? ActionTaken { get; set; }
}
