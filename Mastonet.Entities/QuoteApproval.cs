using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Mastonet.Entities;

/// <summary>
/// Represents the approval state of a quote.
/// https://docs.joinmastodon.org/entities/QuoteApproval
/// </summary>
public class QuoteApproval
{
    /// <summary>
    /// Describes who is expected to be able to quote that status and have the quote automatically authorized.
    /// An empty list means that nobody is expected to be able to quote this post. Other values may be added in the future,
    /// so unknown values should be treated as unsupported_policy.
    /// </summary>
    [JsonPropertyName("automatic")]
    public IEnumerable<QuoteApprovalPolicy> Automatic { get; set; } = Enumerable.Empty<QuoteApprovalPolicy>();

    /// <summary>
    /// Describes who is expected to have their quotes of this status be manually reviewed by the author before being accepted.
    /// An empty list means that nobody is expected to be able to quote this post. Other values may be added in the future,
    /// so unknown values should be treated as unsupported_policy.
    /// </summary>
    [JsonPropertyName("manual")]
    public IEnumerable<QuoteApprovalPolicy> Manual { get; set; } = Enumerable.Empty<QuoteApprovalPolicy>();

    /// <summary>
    /// Describes how this status’ quote policy applies to the current user.
    /// </summary>
    [JsonPropertyName("current_user")]
    public UserQuoteOption CurrentUser { get; set; } = UserQuoteOption.Unknown;
}
