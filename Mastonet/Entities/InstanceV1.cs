using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents the software instance of Mastodon running on this domain.
/// </summary>
[Obsolete("This entity was deprecated in MAstodon v4")]
public class Instance
{
    /// <summary>
    /// The domain name of the instance.
    /// </summary>
    [JsonProperty("uri")]
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// The title of the website.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Admin-defined description of the Mastodon site.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// A shorter description defined by the admin.
    /// </summary>
    [JsonProperty("short_description")]
    public string ShortDescription { get; set; }=string.Empty;

    /// <summary>
    /// An email that may be contacted for any inquiries.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The version of Mastodon installed on the instance.
    /// </summary>
    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Primary languages of the website and its staff.
    /// </summary>
    [JsonProperty("languages")]
    public IEnumerable<string> Languages { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Whether registrations are enabled.
    /// </summary>
    [JsonProperty("registrations")]
    public bool RegistrationsEnabled { get; set; }

    /// <summary>
    /// Whether registrations require moderator approval.
    /// </summary>
    [JsonProperty("approval_required")]
    public bool ApprovalRequired { get; set; }

    /// <summary>
    /// Whether invites are enabled.
    /// </summary>
    [JsonProperty("invites_enabled")]
    public bool InvitesEnabled { get; set;}

    /// <summary>
    /// URLs of interest for clients apps.
    /// </summary>
    [JsonProperty("urls")]
    public InstanceUrls Urls { get; set; } = new InstanceUrls();

    /// <summary>
    /// Statistics about how much information the instance contains.
    /// </summary>
    [JsonProperty("stats")]
    public InstanceStats Stats { get; set; } = new InstanceStats();

    /// <summary>
    /// Banner image for the website.
    /// </summary>
    [JsonProperty("thumbnail")]
    public string? Thumbnail { get; set; }

    /// <summary>
    /// A user that can be contacted, as an alternative to email.
    /// </summary>
    [JsonProperty("contact_account")]
    public Account? ContactAccount { get; set; }
}

public class InstanceUrls
{
    /// <summary>
    /// Websocket base URL for streaming API
    /// </summary>
    [JsonProperty("streaming_api")]
    public string StreamingAPI { get; set; } = string.Empty;
}

public class InstanceStats
{
    /// <summary>
    /// Users registered on this instance. 
    /// </summary>
    [JsonProperty("user_count")]
    public long UserCount { get; set; }

    /// <summary>
    /// Statuses authored by users on instance. 
    /// </summary>
    [JsonProperty("status_count")]
    public long StatusCount { get; set; }

    /// <summary>
    /// Domains federated with this instance. 
    /// </summary>
    [JsonProperty("domain_count")]
    public long DomainCount { get; set; }
}
