using System;
using Newtonsoft.Json;

namespace Mastonet.Entities;

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