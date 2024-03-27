using System.Text.Json.Serialization;

namespace Mastonet.Entities;

/// <summary>
/// Represents daily usage history of a hashtag.
/// </summary>
public class History
{
    /// <summary>
    /// UNIX timestamp on midnight of the given day.
    /// </summary>
    [JsonPropertyName("day")]
    public string Day { get; set; } = string.Empty;

    /// <summary>
    /// the counted usage of the tag within that day.
    /// </summary>
    [JsonPropertyName("uses")]
    public string Uses { get; set; } = string.Empty;

    /// <summary>
    /// the total of accounts using the tag within that day.
    /// </summary>
    [JsonPropertyName("accounts")]
    public string Accounts { get; set; } = string.Empty;
}