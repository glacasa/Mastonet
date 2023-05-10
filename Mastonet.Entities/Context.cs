using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents the tree around a given status. Used for reconstructing threads of statuses.
/// </summary>
public class Context
{
    /// <summary>
    /// Parents in the thread.
    /// </summary>
    [JsonPropertyName("ancestors")]
    public IEnumerable<Status> Ancestors { get; set; } = Enumerable.Empty<Status>();

    /// <summary>
    /// Children in the thread.
    /// </summary>
    [JsonPropertyName("descendants")]
    public IEnumerable<Status> Descendants { get; set; } = Enumerable.Empty<Status>();
}
