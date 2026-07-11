using System.Text.Json.Serialization;

namespace Mastonet.Entities;

/// <summary>
/// Represents a quote of a status.
/// https://docs.joinmastodon.org/entities/Quote
/// </summary>
public class Quote
{
    /// <summary>
    /// The state of the quote.
    /// </summary>
    [JsonPropertyName("state")]
    public QuoteState State { get; set; } = QuoteState.Unauthorized;

    /// <summary>
    /// The status that is being quoted.
    /// </summary>
    [JsonPropertyName("quoted_status")]
    public Status? QuotedStatus { get; set; } = null;
}
