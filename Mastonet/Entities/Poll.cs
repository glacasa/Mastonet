using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastonet.Entities
{
    public class Poll
    {
        /// <summary>
        /// The poll ID
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The due DateTime
        /// </summary>
        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Whether the poll is expired
        /// </summary>
        [JsonProperty("expired")]
        public bool Expired { get; set; }

        /// <summary>
        /// Whether a vote for multiple options is accepted
        /// </summary>
        [JsonProperty("multiple")]
        public bool Multiple { get; set; }

        /// <summary>
        /// The total number of votes
        /// </summary>
        [JsonProperty("votes_count")]
        public int VotesCount { get; set; }

        /// <summary>
        /// The array of options
        /// </summary>
        [JsonProperty("options")]
        public IEnumerable<PollOption> Options { get; set; } = Enumerable.Empty<PollOption>();

        /// <summary>
        /// Whether the account has voted
        /// </summary>
        [JsonProperty("voted")]
        public bool? Voted { get; set; }
    }

    public class PollOption
    {
        /// <summary>
        /// The options' title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The number of votes for the option
        /// </summary>
        [JsonProperty("votes_count")]
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
}
