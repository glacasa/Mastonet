using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastonet.Entities;


/// <summary>
/// Represents the results of a search.
/// </summary>
public class SearchResults
{
    /// <summary>
    /// Accounts which match the given query
    /// </summary>
    [JsonProperty("accounts")]
    public IEnumerable<Account> Accounts { get; set; } = Enumerable.Empty<Account>();

    /// <summary>
    /// Statuses which match the given query
    /// </summary>
    [JsonProperty("statuses")]
    public IEnumerable<Status> Statuses { get; set; } = Enumerable.Empty<Status>();

    /// <summary>
    /// Hashtags which match the given query
    /// </summary>
    [JsonProperty("hashtags")]
    public IEnumerable<Tag> Hashtags { get; set; } = Enumerable.Empty<Tag>();
}
