using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastonet.Entities;

/// <summary>
/// Represents a mastodon account retrieved with Admin permissions
/// using the /admin/accounts endpoint
/// </summary>
public class AdminAccount
{
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
    ///  The domain of the account, if it is remote.
    /// </summary>
    [JsonProperty("domain")]
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// When the account was first discovered.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The email address associated with the account.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The IP address last used to login to this account.
    /// </summary>
    [JsonProperty("ip")]
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// The current role of the account.
    /// </summary>
    [JsonProperty("role")]
    public Role? Role { get; set; }

    /// <summary>
    /// Whether the account has confirmed their email address.
    /// </summary>
    [JsonProperty("confirmed")]
    public bool Confirmed { get; set; }

    /// <summary>
    /// Whether the account is currently suspended.
    /// </summary>
    [JsonProperty("suspended")]
    public bool Suspended { get; set; }

    /// <summary>
    /// Whether the account has been force-marked as sensitive.
    /// </summary>
    [JsonProperty("sensitized")]
    public bool Sensitized { get; set; }

    /// <summary>
    /// Whether the account is currently silenced.
    /// </summary>
    [JsonProperty("silenced")]
    public bool Silenced { get; set; }

    /// <summary>
    /// Whether the account is currently disabled.
    /// </summary>
    [JsonProperty("disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Whether the account is currently approved.
    /// </summary>
    [JsonProperty("approved")]
    public bool Approved { get; set; }

    /// <summary>
    /// The locale of the account.
    /// </summary>
    [JsonProperty("locale")]
    public string Locale { get; set; } = string.Empty;

    /// <summary>
    /// The reason given when requesting an invite (for instances that require manual approval of registrations)
    /// </summary>
    [JsonProperty("invite_request")]
    public string InviteRequest { get; set; } = string.Empty;

    /// <summary>
    /// All known IP addresses associated with this account.
    /// </summary>
    [JsonProperty("ips")]
    public IEnumerable<AccountIp> Ips { get; set; } = Enumerable.Empty<AccountIp>();

    /// <summary>
    /// User-level information about the account.
    /// </summary>
    [JsonProperty("account")]
    public Account? Account { get; set; }

    /// <summary>
    /// The ID of the Application that created this account, if applicable.
    /// </summary>
    [JsonProperty("created_by_application_id")]
    public string CreatedByApplicationId { get; set; } = string.Empty;
}

/// <summary>
/// Represents a custom user role that grants permissions.
/// </summary>
public class Role
{
    /// <summary>
    /// The ID of the Role in the database.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the role.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The hex code assigned to this role. If no hex code is assigned, the string will be empty.
    /// </summary>
    [JsonProperty("color")]
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// An index for the role’s position. The higher the position, the more priority the role has over other roles.
    /// </summary>
    [JsonProperty("position")]
    public int Position { get; set; }

    /// <summary>
    /// A bitmask that represents the sum of all permissions granted to the role.
    /// </summary>
    [JsonProperty("permissions")]
    public int Permissions { get; set; }

    /// <summary>
    /// Whether the role is publicly visible as a badge on user profiles.
    /// </summary>
    [JsonProperty("highlighted")]
    public bool Highlighted { get; set; }

    /// <summary>
    /// The date that the role was created.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date that the role was updated.
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Represents an IP address associated with a user.
/// </summary>
public class AccountIp
{
    /// <summary>
    /// The IP address.
    /// </summary>
    [JsonProperty("ip")]
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp of when the IP address was last used for this account.
    /// </summary>
    [JsonProperty("used_at")]
    public DateTime UsedAt { get; set; }
}

