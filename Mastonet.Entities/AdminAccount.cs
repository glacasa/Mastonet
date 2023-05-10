using System.Text.Json.Serialization;
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
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The username of the account, not including domain.
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///  The domain of the account, if it is remote.
    /// </summary>
    [JsonPropertyName("domain")]
    public string? Domain { get; set; } 

    /// <summary>
    /// When the account was first discovered.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The email address associated with the account.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The IP address last used to login to this account.
    /// </summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; set; }

    /// <summary>
    /// The current role of the account.
    /// </summary>
    [JsonPropertyName("role")]
    public Role Role { get; set; } = default!;

    /// <summary>
    /// Whether the account has confirmed their email address.
    /// </summary>
    [JsonPropertyName("confirmed")]
    public bool Confirmed { get; set; }

    /// <summary>
    /// Whether the account is currently suspended.
    /// </summary>
    [JsonPropertyName("suspended")]
    public bool Suspended { get; set; }

    /// <summary>
    /// Whether the account has been force-marked as sensitive.
    /// </summary>
    [JsonPropertyName("sensitized")]
    public bool Sensitized { get; set; }

    /// <summary>
    /// Whether the account is currently silenced.
    /// </summary>
    [JsonPropertyName("silenced")]
    public bool Silenced { get; set; }

    /// <summary>
    /// Whether the account is currently disabled.
    /// </summary>
    [JsonPropertyName("disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Whether the account is currently approved.
    /// </summary>
    [JsonPropertyName("approved")]
    public bool Approved { get; set; }

    /// <summary>
    /// The locale of the account.
    /// </summary>
    [JsonPropertyName("locale")]
    public string Locale { get; set; } = string.Empty;

    /// <summary>
    /// The reason given when requesting an invite (for instances that require manual approval of registrations)
    /// </summary>
    [JsonPropertyName("invite_request")]
    public string? InviteRequest { get; set; } 

    /// <summary>
    /// All known IP addresses associated with this account.
    /// </summary>
    [JsonPropertyName("ips")]
    public IEnumerable<AccountIp> Ips { get; set; } = Enumerable.Empty<AccountIp>();

    /// <summary>
    /// User-level information about the account.
    /// </summary>
    [JsonPropertyName("account")]
    public Account? Account { get; set; }

    /// <summary>
    /// The ID of the Application that created this account, if applicable.
    /// </summary>
    [JsonPropertyName("created_by_application_id")]
    public string? CreatedByApplicationId { get; set; } 
    
    /// <summary>
    /// The ID of the Account that invited this user, if applicable.
    /// </summary>
    [JsonPropertyName("invited_by_account_id")]
    public string? InvitedByAccountId { get; set; } 
}