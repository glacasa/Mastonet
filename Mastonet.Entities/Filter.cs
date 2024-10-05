using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a user-defined filter for determining which statuses should not be shown to the user.
/// </summary>
public class Filter
{
    /// <summary>
    /// The ID of the filter in the database.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// A title given by the user to name the filter.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The text to be filtered.
    /// </summary>
    [JsonPropertyName("phrase")]
    public string Phrase { get; set; } = string.Empty;

    /// <summary>
    /// The contexts in which the filter should be applied.
    /// </summary>
    [JsonPropertyName("context")]
    public FilterContext Context { get; set; }

    /// <summary>
    /// When the filter should no longer be applied
    /// </summary>
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Should matching entities in home and notifications be dropped by the server?
    /// </summary>
    [JsonPropertyName("irreversible")]
    public bool Irreversible { get; set; }

    /// <summary>
    /// Should the filter consider word boundaries?
    /// </summary>
    /// <remarks>
    /// If whole_word is true , client app should do:
    /// Define ‘word constituent character’ for your app. In the official implementation, it’s [A-Za-z0-9_] in JavaScript, and [[:word:]] in Ruby. Ruby uses the POSIX character class (Letter | Mark | Decimal_Number | Connector_Punctuation).
    /// If the phrase starts with a word character, and if the previous character before matched range is a word character, its matched range should be treated to not match.
    /// If the phrase ends with a word character, and if the next character after matched range is a word character, its matched range should be treated to not match.
    /// Please check app/javascript/mastodon/selectors/index.js and app/lib/feed_manager.rb in the Mastodon source code for more details.
    /// </remarks>
    [JsonPropertyName("whole_word")]
    public bool WholeWord { get; set; }
}
