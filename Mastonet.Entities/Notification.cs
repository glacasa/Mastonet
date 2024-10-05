using System.Text.Json.Serialization;
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
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The type of event that resulted in the notification. One of: 
    /// mention = Someone mentioned you in their status
    /// status = Someone you enabled notifications for has posted a status
    /// reblog = Someone boosted one of your statuses
    /// follow = Someone followed you
    /// follow_request = Someone requested to follow you
    /// favourite = Someone favourited one of your statuses
    /// poll = A poll you have voted in or created has ended
    /// update = A status you interacted with has been edited
    /// admin.sign_up = Someone signed up (optionally sent to admins)
    /// admin.report = A new report has been filed
    /// severed_relationships = Some of your follow relationships have been severed as a result of a moderation or block event
    /// moderation_warning = A moderator has taken action against your account or has sent you a warning
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("group_key")]
    public string GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp of the notification.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The account that performed the action that generated the notification.
    /// </summary>
    [JsonPropertyName("account")]
    public Account Account { get; set; } = new Account();

    /// <summary>
    /// Status that was the object of the notification, e.g. in mentions, reblogs, favourites, or polls.
    /// </summary>
    [JsonPropertyName("status")]
    public Status? Status { get; set; }

    /// <summary>
    /// Summary of the event that caused follow relationships to be severed. Attached when type of the notification is severed_relationships.
    /// </summary>
    [JsonPropertyName("relationship_severance_event")]
    public RelationshipSeveranceEvent RelationshipSeveranceEvent { get; set; } = new RelationshipSeveranceEvent();

    /// <summary>
    /// Moderation warning that caused the notification. Attached when type of the notification is moderation_warning.
    /// </summary>
    [JsonPropertyName("moderation_warning")]
    public AccountWarning ModerationWarning { get; set; } = new AccountWarning();
}
