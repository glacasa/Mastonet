using Newtonsoft.Json;

namespace Mastonet.Entities;

/// <summary>
/// Represents daily usage history of a hashtag.
/// </summary>
public class History
{
    /// <summary>
    /// UNIX timestamp on midnight of the given day.
    /// </summary>
    [JsonProperty("day")]
    public int Day { get; set; }

    /// <summary>
    /// the counted usage of the tag within that day.
    /// </summary>
    [JsonProperty("uses")]
    public int Uses { get; set; }

    /// <summary>
    /// the total of accounts using the tag within that day.
    /// </summary>
    [JsonProperty("accounts")]
    public int Accounts { get; set; }
}
