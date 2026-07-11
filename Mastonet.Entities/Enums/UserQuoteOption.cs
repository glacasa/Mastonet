using System.Text.Json.Serialization;

namespace Mastonet;

/// <summary>
/// Represents the option for how a user's status can be quoted.
/// https://docs.joinmastodon.org/entities/QuoteApproval/#current_user
/// Version: 4.5.0
/// </summary>
[JsonConverter(typeof(JsonEnumConverter<UserQuoteOption>))]
public enum UserQuoteOption
{
    /// <summary>
    /// The requesting user is expected to be allowed to quote and have their quote be automatically accepted.
    /// </summary>
    Automatic,

    /// <summary>
    /// The requesting user is expected to be allowed to quote after manual review of the post by the quoted status’ author.
    /// </summary>
    Manual,

    /// <summary>
    /// The requesting user is not expected to be allowed to quote this post. Mastodon will return an error if you attempt to do so.
    /// </summary>
    Denied,

    /// <summary>
    /// The user is not covered by the quote policies supported by Mastodon,
    /// and there are additional underlying quote policies that are unsupported by Mastodon.
    /// This should be treated as denied unless you are targeting “power users”.
    /// </summary>
    Unknown,
}
