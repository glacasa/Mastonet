using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastonet.Entities;

/// <summary>
/// Represents a user of Mastodon and their associated profile
/// </summary>
public class Account
{
    // Base Attributes

    /// <summary>
    /// The account id.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The username of the account, not including domain.
    /// </summary>
    [JsonProperty("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The Webfinger account URI. Equal to username for local users, or username@domain for remote users.
    /// </summary>
    [JsonProperty("acct")]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// The location of the user's profile page.
    /// </summary>
    [JsonProperty("url")]
    public string ProfileUrl { get; set; } = string.Empty;

    // Display attributes

    /// <summary>
    /// The profile's display name.
    /// </summary>
    [JsonProperty("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The profile's bio / description.
    /// </summary>
    [JsonProperty("note")]
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// An image icon that is shown next to statuses and in the profile.
    /// </summary>
    [JsonProperty("avatar")]
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// A static version of the avatar. Equal to avatar if its value is a static image; different if avatar is an animated GIF.
    /// </summary>
    [JsonProperty("avatar_static")]
    public string StaticAvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// An image banner that is shown above the profile and in profile cards.
    /// </summary>
    [JsonProperty("header")]
    public string HeaderUrl { get; set; } = string.Empty;

    /// <summary>
    /// A static version of the header. Equal to header if its value is a static image; different if header is an animated GIF.
    /// </summary>
    [JsonProperty("header_static")]
    public string StaticHeaderUrl { get; set; } = string.Empty;

    /// <summary>
    /// Whether the account manually approves follow requests.
    /// </summary>
    [JsonProperty("locked")]
    public bool Locked { get; set; }

    /// <summary>
    /// Custom emoji entities to be used when rendering the profile. If none, an empty array will be returned.
    /// </summary>
    [JsonProperty("emojis")]
    public IEnumerable<Emoji> Emojis { get; set; } = Enumerable.Empty<Emoji>();

    /// <summary>
    /// Whether the account has opted into discovery features such as the profile directory.
    /// </summary>
    [JsonProperty("discoverable")]
    public bool? Discoverable { get; set; }

    // Statistical attributes

    /// <summary>
    /// The time the account was created
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the most recent status was posted.
    /// </summary>
    [JsonProperty("last_status_at")]
    public DateTime? LastStatusAt { get; set; }

    /// <summary>
    /// How many statuses are attached to this account.
    /// </summary>
    [JsonProperty("statuses_count")]
    public long StatusesCount { get; set; }

    /// <summary>
    /// The reported followers of this profile.
    /// </summary>
    [JsonProperty("followers_count")]
    public long FollowersCount { get; set; }

    /// <summary>
    /// The reported follows of this profile.
    /// </summary>
    [JsonProperty("following_count")]
    public long FollowingCount { get; set; }

    // Optional attributes

    /// <summary>
    /// Indicates that the profile is currently inactive and that its user has moved to a new account.
    /// </summary>
    [JsonProperty("moved")]
    public Account? Moved { get; set; }

    /// <summary>
    /// Additional metadata attached to a profile as name-value pairs.
    /// </summary>
    [JsonProperty("fields")]
    public IEnumerable<Field>? Fields { get; set; }

    /// <summary>
    /// A presentational flag. Indicates that the account may perform automated actions, may not be monitored, or identifies as a robot.
    /// </summary>
    [JsonProperty("bot")]
    public bool? Bot { get; set; }

    /// <summary>
    /// An extra entity to be used with API methods to verify credentials and update credentials.
    /// </summary>
    [JsonProperty("source")]
    public Source? Source { get; set; }

    /// <summary>
    /// An extra entity returned when an account is suspended.
    /// </summary>
    [JsonProperty("suspended")]
    public bool Suspended { get; set; }

    /// <summary>
    /// When a timed mute will expire, if applicable.
    /// </summary>
    [JsonProperty("mute_expires_at")]
    public DateTime? MuteExpiresAt { get; set; }
}
