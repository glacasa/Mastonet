﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    // https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md#status
    public class Status
    {
        /// <summary>
        /// The ID of the status
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// A Fediverse-unique resource ID
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// URL to the status page (can be remote)
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }

        /// <summary>
        /// The Account which posted the status
        /// </summary>
        [JsonProperty("account")]
        public Account Account { get; set; } = new Account();

        /// <summary>
        /// null or the ID of the status it replies to
        /// </summary>
        [JsonProperty("in_reply_to_id")]
        public long? InReplyToId { get; set; }

        /// <summary>
        /// null or the ID of the account it replies to
        /// </summary>
        [JsonProperty("in_reply_to_account_id")]
        public long? InReplyToAccountId { get; set; }

        /// <summary>
        /// null or the reblogged Status
        /// </summary>
        [JsonProperty("reblog")]
        public Status? Reblog { get; set; }

        /// <summary>
        /// Body of the status; this will contain HTML (remote HTML already sanitized)
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The time the status was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// An array of Emoji
        /// </summary>
        [JsonProperty("emojis")]
        public IEnumerable<Emoji> Emojis { get; set; } = new Emoji[0];
        
        /// <summary>
        /// The number of replies for the status
        /// </summary>
        [JsonProperty("replies_count")]
        public int RepliesCount { get; set; }

        /// <summary>
        /// The number of reblogs for the status
        /// </summary>
        [JsonProperty("reblogs_count")]
        public int ReblogCount { get; set; }

        /// <summary>
        /// The number of favourites for the status
        /// </summary>
        [JsonProperty("favourites_count")]
        public int FavouritesCount { get; set; }

        /// <summary>
        /// Whether the authenticated user has reblogged the status
        /// </summary>
        [JsonProperty("reblogged")]
        public bool? Reblogged { get; set; }

        /// <summary>
        /// Whether the authenticated user has favourited the status
        /// </summary>
        [JsonProperty("favourited")]
        public bool? Favourited { get; set; }

        /// <summary>
        /// Whether the authenticated user has muted the conversation this status from
        /// </summary>
        [JsonProperty("muted")]
        public bool? Muted { get; set; }

        /// <summary>
        /// Whether media attachments should be hidden by default
        /// </summary>
        [JsonProperty("sensitive")]
        public bool Sensitive { get; set; }

        /// <summary>
        /// If not empty, warning text that should be displayed before the actual content
        /// </summary>
        [JsonProperty("spoiler_text")]
        public string SpoilerText { get; set; } = string.Empty;

        /// <summary>
        /// One of: public, unlisted, private, direct
        /// </summary>
        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }

        /// <summary>
        /// An array of Attachments
        /// </summary>
        [JsonProperty("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; } = new Attachment[0];

        /// <summary>
        /// An array of Mentions
        /// </summary>
        [JsonProperty("mentions")]
        public IEnumerable<Mention> Mentions { get; set; } = new Mention[0];

        /// <summary>
        /// An array of Tags
        /// </summary>
        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; } = new Tag[0];

        /// <summary>
        /// Application from which the status was posted
        /// </summary>
        [JsonProperty("application")]
        public Application? Application { get; set; }

        /// <summary>
        /// The detected language for the status, if detected
        /// </summary>
        [JsonProperty("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Whether this is the pinned status for the account that posted it
        /// </summary>
        [JsonProperty("pinned")]
        public bool? Pinned { get; set; }

    }
}
