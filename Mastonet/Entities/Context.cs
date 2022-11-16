using Newtonsoft.Json;
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
    [JsonProperty("ancestors")]
    public IEnumerable<Status> Ancestors { get; set; } = Enumerable.Empty<Status>();

    /// <summary>
    /// Children in the thread.
    /// </summary>
    [JsonProperty("descendants")]
    public IEnumerable<Status> Descendants { get; set; } = Enumerable.Empty<Status>();
}
