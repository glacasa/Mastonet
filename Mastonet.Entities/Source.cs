using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Mastonet.Entities;

/// <summary>
/// Represents display or publishing preferences of user's own account. 
/// Returned as an additional entity when verifying and updated credentials, as an attribute of Account.
/// </summary>
public class Source
{        
    /// <summary>
    /// Profile bio.
    /// </summary>
    [JsonPropertyName("note")]
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// Metadata about the account.
    /// </summary>
    [JsonPropertyName("fields")]
    public IEnumerable<Field> Fields { get; set; } = Enumerable.Empty<Field>();

    // Nullable attributes

    /// <summary>
    /// The default post privacy to be used for new statuses.
    /// </summary>
    [JsonPropertyName("privacy")]
    public Visibility? Privacy { get; set; }

    /// <summary>
    /// Whether new statuses should be marked sensitive by default.
    /// </summary>
    [JsonPropertyName("sensitive")]
    public bool? Sensitive { get; set; }

    /// <summary>
    /// The default posting language for new statuses.
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// The number of pending follow requests.
    /// </summary>
    [JsonPropertyName("follow_requests_count")]
    public int? FollowRequestsCount { get; set; }
}
