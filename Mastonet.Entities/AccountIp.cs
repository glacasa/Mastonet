using System;
using System.Text.Json.Serialization;

namespace Mastonet.Entities;

/// <summary>
/// Represents an IP address associated with a user.
/// </summary>
public class AccountIp
{
    /// <summary>
    /// The IP address.
    /// </summary>
    [JsonPropertyName("ip")]
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp of when the IP address was last used for this account.
    /// </summary>
    [JsonPropertyName("used_at")]
    public DateTime UsedAt { get; set; }
}