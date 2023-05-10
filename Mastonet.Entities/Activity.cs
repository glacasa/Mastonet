using System.Text.Json.Serialization;

namespace Mastonet.Entities;

/// <summary>
/// Represents a weekly bucket of instance activity.
/// </summary>
public class Activity
{
    /// <summary>
    /// Midnight at the first day of the week.
    /// </summary>
    [JsonPropertyName("week")]
    public string Week { get; set; } = string.Empty;

    /// <summary>
    /// Statuses created since the week began.
    /// </summary>
    [JsonPropertyName("statuses")]
    public long Statuses { get; set; }

    /// <summary>
    /// User logins since the week began.
    /// </summary>
    [JsonPropertyName("logins")]
    public long Logins { get; set; }

    /// <summary>
    /// User registrations since the week began.
    /// </summary>
    [JsonPropertyName("registrations")]
    public long Registrations { get; set; }
}
