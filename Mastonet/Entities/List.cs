using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

/// <summary>
/// Represents a list of some users that the authenticated user follows.
/// </summary>
public class List
{
    /// <summary>
    /// The internal database ID of the list.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The user-defined title of the list.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Which replies should be shown in the list. One of :
    /// followed = Show replies to any followed user
    /// list = Show replies to members of the list
    /// none = Show replies to no one
    /// </summary>
    [JsonProperty("replies_policy")]
    public string RepliesPolicy { get; set; }= string.Empty;
}
