using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Reports filed against users and/or statuses, to be taken action on by moderators.
/// </summary>
public class Report
{
    /// <summary>
    /// The ID of the report
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///  Whether an action was taken yet.
    /// </summary>
    [JsonPropertyName("action_taken")]
    public bool ActionTaken { get; set; }

    /// <summary>
    /// When an action was taken against the report.
    /// </summary>
    [JsonPropertyName("action_taken_at")]
    public string? ActionTakenAt { get; set; }

    /// <summary>
    /// The generic reason for the report. One of:
    /// spam = Unwanted or repetitive content
    /// violation = A specific rule was violated
    /// other = Some other reason
    /// </summary>
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The reason for the report.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Whether the report was forwarded to a remote domain.
    /// </summary>
    [JsonPropertyName("forwarded")]
    public bool Forwarded { get; set; }

    /// <summary>
    /// When the report was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// IDs of statuses that have been attached to this report for additional context.
    /// </summary>
    [JsonPropertyName("status_ids")]
    public IEnumerable<string>? StatusIds { get; set; }

    /// <summary>
    /// IDs of the rules that have been cited as a violation by this report.
    /// </summary>
    [JsonPropertyName("rule_ids")]
    public IEnumerable<string>? RuleIds { get; set; }

    /// <summary>
    /// The account that was reported.
    /// </summary>
    [JsonPropertyName("target_account")]
    public Account TargetAccount { get; set; } = new Account();
}
