using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Mastonet.Entities;

/// <summary>
/// Represents a conversation with "direct message" visibility.
/// </summary>
public class Conversation
{
    /// <summary>
    /// Local database ID of the conversation.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Participants in the conversation.
    /// </summary>
    [JsonProperty("accounts")]
    public IEnumerable<Account> Accounts { get; set; } = Enumerable.Empty<Account>();

    /// <summary>
    /// Is the conversation currently marked as unread?
    /// </summary>
    [JsonProperty("unread")]
    public bool Unread { get; set; }

    /// <summary>
    /// The last status in the conversation, to be used for optional display.
    /// </summary>
    [JsonProperty("last_status")]
    public Status? LastStatus { get; set; }
}
