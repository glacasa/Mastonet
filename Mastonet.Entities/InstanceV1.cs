using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents the software instance of Mastodon running on this domain.
/// </summary>
[Obsolete("This entity was deprecated in Mastodon v4")]
public class Instance
{
    /// <summary>
    /// The domain name of the instance.
    /// </summary>
    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// The title of the website.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Admin-defined description of the Mastodon site.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// A shorter description defined by the admin.
    /// </summary>
    [JsonPropertyName("short_description")]
    public string ShortDescription { get; set; }=string.Empty;

    /// <summary>
    /// An email that may be contacted for any inquiries.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The version of Mastodon installed on the instance.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Primary languages of the website and its staff.
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<string> Languages { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Whether registrations are enabled.
    /// </summary>
    [JsonPropertyName("registrations")]
    public bool RegistrationsEnabled { get; set; }

    /// <summary>
    /// Whether registrations require moderator approval.
    /// </summary>
    [JsonPropertyName("approval_required")]
    public bool ApprovalRequired { get; set; }

    /// <summary>
    /// Whether invites are enabled.
    /// </summary>
    [JsonPropertyName("invites_enabled")]
    public bool InvitesEnabled { get; set;}

    /// <summary>
    /// URLs of interest for clients apps.
    /// </summary>
    [JsonPropertyName("urls")]
    public InstanceUrls Urls { get; set; } = new InstanceUrls();

    /// <summary>
    /// Statistics about how much information the instance contains.
    /// </summary>
    [JsonPropertyName("stats")]
    public InstanceStats Stats { get; set; } = new InstanceStats();

    /// <summary>
    /// Banner image for the website.
    /// </summary>
    [JsonPropertyName("thumbnail")]
    public string? Thumbnail { get; set; }

    /// <summary>
    /// A user that can be contacted, as an alternative to email.
    /// </summary>
    [JsonPropertyName("contact_account")]
    public Account? ContactAccount { get; set; }
}

public class InstanceUrls
{
    /// <summary>
    /// Websocket base URL for streaming API
    /// </summary>
    [JsonPropertyName("streaming_api")]
    public string StreamingAPI { get; set; } = string.Empty;
}

public class InstanceStats
{
    /// <summary>
    /// Users registered on this instance. 
    /// </summary>
    [JsonPropertyName("user_count")]
    public long UserCount { get; set; }

    /// <summary>
    /// Statuses authored by users on instance. 
    /// </summary>
    [JsonPropertyName("status_count")]
    public long StatusCount { get; set; }

    /// <summary>
    /// Domains federated with this instance. 
    /// </summary>
    [JsonPropertyName("domain_count")]
    public long DomainCount { get; set; }
}
