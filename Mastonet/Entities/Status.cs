using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class Status
    {
        /// <summary>
        /// The ID of the status
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// A Fediverse-unique resource ID
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// URL to the status page (can be remote)
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The Account which posted the status
        /// </summary>
        [JsonProperty("account")]
        public Account Account { get; set; }

        /// <summary>
        /// null or the ID of the status it replies to
        /// </summary>
        [JsonProperty("in_reply_to_id")]
        public int? InReplyToId { get; set; }

        /// <summary>
        /// null or the ID of the account it replies to
        /// </summary>
        [JsonProperty("in_reply_to_account_id")]
        public int? InReplyToAccountId { get; set; }

        /// <summary>
        /// null or the reblogged Status
        /// </summary>
        [JsonProperty("reblog")]
        public Status Reblog { get; set; }

        /// <summary>
        /// Body of the status; this will contain HTML (remote HTML already sanitized)
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// The time the status was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

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
        /// Whether media attachments should be hidden by default
        /// </summary>
        [JsonProperty("sensitive")]
        public bool? Sensitive { get; set; }

        /// <summary>
        /// If not empty, warning text that should be displayed before the actual content
        /// </summary>
        [JsonProperty("spoiler_text")]
        public string SpoilerText { get; set; }

        /// <summary>
        /// One of: public, unlisted, private, direct
        /// </summary>
        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        /// <summary>
        /// An array of Attachments
        /// </summary>
        [JsonProperty("media_attachments")]
        public IEnumerable<Attachment> MediaAttachments { get; set; }

        /// <summary>
        /// An array of Mentions
        /// </summary>
        [JsonProperty("mentions")]
        public IEnumerable<Mention> Mentions { get; set; }

        /// <summary>
        /// An array of Tags
        /// </summary>
        [JsonProperty("tags")]
        public IEnumerable<Tag> Tags { get; set; }

        /// <summary>
        /// Application from which the status was posted
        /// </summary>
        [JsonProperty("application")]
        public Application Application { get; set; }

    }
}
