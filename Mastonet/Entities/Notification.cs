using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a notification of an event relevant to the user.
/// </summary>
public class Notification
{
    /// <summary>
    /// The id of the notification in the database.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The type of event that resulted in the notification. One of: 
    /// follow = Someone followed you
    /// follow_request = Someone requested to follow you
    /// mention = Someone mentioned you in their status
    /// reblog = Someone boosted one of your statuses
    /// favourite = Someone favourited one of your statuses
    /// poll = A poll you have voted in or created has ended
    /// status = Someone you enabled notifications for has posted a status
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp of the notification.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The account that performed the action that generated the notification.
    /// </summary>
    [JsonProperty("account")]
    public Account Account { get; set; } = new Account();

    /// <summary>
    /// Status that was the object of the notification, e.g. in mentions, reblogs, favourites, or polls.
    /// </summary>
    [JsonProperty("status")]
    public Status? Status { get; set; }
}
