using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a proof from an external identity provider.
/// </summary>
public class IdentityProof
{
    /// <summary>
    /// The name of the identity provider.
    /// </summary>
    [JsonProperty("provider")]
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// The account owner's username on the identity provider's service.
    /// </summary>
    [JsonProperty("provider_username")]
    public string ProviderUsername { get; set; } = string.Empty;

    /// <summary>
    /// The account owner's profile URL on the identity provider.
    /// </summary>
    [JsonProperty("profile_url")]
    public string ProfileUrl { get; set; } = string.Empty;

    /// <summary>
    /// A link to a statement of identity proof, hosted by the identity provider.
    /// </summary>
    [JsonProperty("proof_url")]
    public string ProofUrl { get; set; }= string.Empty;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set;}
}
