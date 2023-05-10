using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastonet.Entities;

/// <summary>
/// Represents a poll attached to a status.
/// </summary>
public class Poll
{
    /// <summary>
    /// The ID of the poll in the database.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// When the poll ends.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Is the poll currently expired?
    /// </summary>
    [JsonPropertyName("expired")]
    public bool Expired { get; set; }

    /// <summary>
    /// Does the poll allow multiple-choice answers?
    /// </summary>
    [JsonPropertyName("multiple")]
    public bool Multiple { get; set; }

    /// <summary>
    /// How many votes have been received.
    /// </summary>
    [JsonPropertyName("votes_count")]
    public long VotesCount { get; set; }

    /// <summary>
    /// How many unique accounts have voted on a multiple-choice poll.
    /// null if Multiple is false
    /// </summary>
    [JsonPropertyName("voters_count")]
    public int? VotersCount { get; set; }

    /// <summary>
    /// When called with a user token, has the authorized user voted?
    /// </summary>
    [JsonPropertyName("voted")]
    public bool? Voted { get; set; }

    /// <summary>
    /// When called with a user token, which options has the authorized user chosen? 
    /// Contains an array of index values for options.
    /// </summary>
    [JsonPropertyName("own_votes")]
    public IEnumerable<int> OwnVotes { get; set; }= Enumerable.Empty<int>();

    /// <summary>
    /// Possible answers for the poll.
    /// </summary>
    [JsonPropertyName("options")]
    public IEnumerable<PollOption> Options { get; set; } = Enumerable.Empty<PollOption>();

    /// <summary>
    /// Custom emoji to be used for rendering poll options.
    /// </summary>
    [JsonPropertyName("emojis")]
    public IEnumerable<Emoji> Emojis { get; set; }= Enumerable.Empty<Emoji>();
}

public class PollOption
{
    /// <summary>
    /// The text value of the poll option. 
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The number of received votes for this option. Number, or null if results are not published yet.
    /// </summary>
    [JsonPropertyName("votes_count")]
    public int? VotesCount { get; set; }
}

public class PollParameters
{
    /// <summary>
    /// The array of options
    /// </summary>
    public IEnumerable<string> Options { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// The timespan until expiration
    /// </summary>
    public TimeSpan ExpiresIn { get; set; }

    /// <summary>
    /// Whether to accept a vote for multiple options
    /// </summary>
    public bool? Multiple { get; set; }

    /// <summary>
    /// Whether to hide the number of votes for each option until expiration
    /// </summary>
    public bool? HideTotals { get; set; }
}
