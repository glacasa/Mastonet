using Newtonsoft.Json;
using System;

namespace Nestodon.Entities
{
    public class Account
    {
        /// <summary>
        /// The ID of the account
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The username of the account
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        /// <summary>
        /// Equals username for local users, includes @domain for remote ones
        /// </summary>
        [JsonProperty(PropertyName = "acct")]
        public string AccountName { get; set; }

        /// <summary>
        /// The account's display name
        /// </summary>
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Biography of user
        /// </summary>
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        /// <summary>
        /// URL of the user's profile page (can be remote)
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string ProfileUrl { get; set; }

        /// <summary>
        /// URL to the avatar image
        /// </summary>
        [JsonProperty(PropertyName = "avatar")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// URL to the header image
        /// </summary>
        [JsonProperty(PropertyName = "header")]
        public string HeaderUrl { get; set; }

        /// <summary>
        /// Boolean for when the account cannot be followed without waiting for approval first
        /// </summary>
        [JsonProperty(PropertyName = "locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// The time the account was created
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The number of followers for the account
        /// </summary>
        [JsonProperty(PropertyName = "followers_count")]
        public int FollowersCount { get; set; }

        /// <summary>
        /// The number of accounts the given account is following
        /// </summary>
        [JsonProperty(PropertyName = "following_count")]
        public int FollowingCount { get; set; }

        /// <summary>
        /// The number of statuses the account has made
        /// </summary>
        [JsonProperty(PropertyName = "statuses_count")]
        public int StatusesCount { get; set; }
    }
}
