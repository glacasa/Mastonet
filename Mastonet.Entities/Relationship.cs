using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents the relationship between accounts, such as following / blocking / muting / etc.
/// </summary>
public class Relationship
{
    /// <summary>
    /// The account id.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Are you following this user?
    /// </summary>
    [JsonProperty("following")]
    public bool Following { get; set; }

    /// <summary>
    /// Do you have a pending follow request for this user?
    /// </summary>
    [JsonProperty("requested")]
    public bool Requested { get; set; }

    /// <summary>
    /// Are you featuring this user on your profile?
    /// </summary>
    [JsonProperty("endorsed")]
    public bool Endorsed { get; set; }

    /// <summary>
    /// Are you followed by this user?
    /// </summary>
    [JsonProperty("followed_by")]
    public bool FollowedBy { get; set; }

    /// <summary>
    /// Are you muting this user?
    /// </summary>
    [JsonProperty("muting")]
    public bool Muting { get; set; }

    /// <summary>
    /// Are you muting notifications from this user?
    /// </summary>
    [JsonProperty("muting_notifications")]
    public bool MutingNotifications { get; set; }

    /// <summary>
    /// Are you receiving this user's boosts in your home timeline?
    /// </summary>
    [JsonProperty("showing_reblogs")]
    public bool ShowingReblogs { get; set; }

    /// <summary>
    /// Have you enabled notifications for this user?
    /// </summary>
    [JsonProperty("notifying")]
    public bool Notifying { get; set; }

    /// <summary>
    /// Are you blocking this user?
    /// </summary>
    [JsonProperty("blocking")]
    public bool Blocking { get; set; }

    /// <summary>
    /// Are you blocking this user's domain?
    /// </summary>
    [JsonProperty("domain_blocking")]
    public bool DomainBlocking { get; set; }

    /// <summary>
    /// Is this user blocking you?
    /// </summary>
    [JsonProperty("blocked_by")]
    public bool BlockedBy { get; set; }

    /// <summary>
    /// This user's profile bio
    /// </summary>
    [JsonProperty("note")]
    public string Note { get; set; } = string.Empty;
}
