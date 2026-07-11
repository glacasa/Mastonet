using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mastonet;

/// <summary>
/// Represents the state of a quote.
/// https://docs.joinmastodon.org/entities/Quote/#state
/// Current version: 4.5.0
/// </summary>
[JsonConverter(typeof(JsonEnumConverter<QuoteState>))]
public enum QuoteState
{
    /// <summary>
    /// The quote has not been acknowledged by the quoted account yet, and requires authorization before being displayed.
    /// </summary>
    Pending,

    /// <summary>
    /// The quote has been accepted and can be displayed. quoted_status is non-null.
    /// </summary>
    Accepted,

    /// <summary>
    /// The quote has been explicitly rejected by the quoted account, and cannot be displayed.
    /// </summary>
    Rejected,

    /// <summary>
    /// The quote has been previously accepted, but is now revoked, and thus cannot be displayed.
    /// </summary>
    Revoked,

    /// <summary>
    /// The quote has been approved, but the quoted post itself has now been deleted.
    /// </summary>
    Deleted,

    /// <summary>
    /// The quote has been approved, but cannot be displayed because the user is not authorized to see it.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// The quote has been approved, but should not be displayed because the user has blocked the account being quoted. quoted_status is non-null.
    /// </summary>
    BlockedAccount,

    /// <summary>
    /// The quote has been approved, but should not be displayed because the user has blocked the domain of the account being quoted. quoted_status is non-null.
    /// </summary>
    BlockedDomain,

    /// <summary>
    /// The quote has been approved, but should not be displayed because the user has muted the the account being quoted. quoted_status is non-null.
    /// </summary>
    MutedAccount,
}
