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
    public string? Domain { get; set; } 

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
    public string? Ip { get; set; }

    /// <summary>
    /// The current role of the account.
    /// </summary>
    [JsonProperty("role")]
    public Role Role { get; set; } = default!;

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
    public string? InviteRequest { get; set; } 

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
    public string? CreatedByApplicationId { get; set; } 
    
    /// <summary>
    /// The ID of the Account that invited this user, if applicable.
    /// </summary>
    [JsonProperty("invited_by_account_id")]
    public string? InvitedByAccountId { get; set; } 
}