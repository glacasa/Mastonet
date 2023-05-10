using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities;

/// <summary>
/// Represents a conversation with "direct message" visibility.
/// </summary>
public class Conversation
{
    /// <summary>
    /// Local database ID of the conversation.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Participants in the conversation.
    /// </summary>
    [JsonPropertyName("accounts")]
    public IEnumerable<Account> Accounts { get; set; } = Enumerable.Empty<Account>();

    /// <summary>
    /// Is the conversation currently marked as unread?
    /// </summary>
    [JsonPropertyName("unread")]
    public bool Unread { get; set; }

    /// <summary>
    /// The last status in the conversation, to be used for optional display.
    /// </summary>
    [JsonPropertyName("last_status")]
    public Status? LastStatus { get; set; }
}
