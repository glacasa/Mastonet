using System.Text.Json.Serialization;

namespace Mastonet;

/// <summary>
/// The default quote policy to be used for new statuses.
/// https://docs.joinmastodon.org/entities/Account/#source-quote_policy
/// Version: 4.5.0
/// </summary>
[JsonConverter(typeof(JsonEnumConverter<QuotePolicy>))]
public enum QuotePolicy
{
    /// <summary>
    /// Anyone (except blocked accounts) can quote.
    /// </summary>
    Public,

    /// <summary>
    /// Only followers and author can quote.
    /// </summary>
    Followers,

    /// <summary>
    /// Only author can quote.
    /// </summary>
    Nobody,
}
