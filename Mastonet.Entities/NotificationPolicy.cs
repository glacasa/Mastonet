using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mastonet.Entities
{
    public class NotificationPolicy
    {
        /// <summary>
        /// Whether to accept, filter or drop notifications from accounts the user is not following. drop will prevent creation of the notification object altogether (without preventing the underlying activity), filter will cause it to be marked as filtered, and accept will not affect its processing.
        /// </summary>
        [JsonPropertyName("for_not_following\r\n")]
        public string ForNotFollowing { get; set; } = string.Empty;

        /// <summary>
        /// Whether to accept, filter or drop notifications from accounts that are not following the user. drop will prevent creation of the notification object altogether (without preventing the underlying activity), filter will cause it to be marked as filtered, and accept will not affect its processing.
        /// </summary>
        [JsonPropertyName("for_not_followers")]
        public string ForNotFollowers { get; set; } = string.Empty;

        /// <summary>
        /// Whether to accept, filter or drop notifications from accounts created in the past 30 days. drop will prevent creation of the notification object altogether (without preventing the underlying activity), filter will cause it to be marked as filtered, and accept will not affect its processing.
        /// </summary>
        [JsonPropertyName("for_new_accounts")]
        public string ForNewAccounts { get; set; } = string.Empty;

        /// <summary>
        /// Whether to accept, filter or drop notifications from private mentions. drop will prevent creation of the notification object altogether (without preventing the underlying activity), filter will cause it to be marked as filtered, and accept will not affect its processing. Replies to private mentions initiated by the user, as well as accounts the user follows, are always allowed, regardless of this value.
        /// </summary>
        [JsonPropertyName("for_private_mentions")]
        public string ForPrivateMentions { get; set; } = string.Empty;

        /// <summary>
        /// Whether to accept, filter or drop notifications from accounts that were limited by a moderator. drop will prevent creation of the notification object altogether (without preventing the underlying activity), filter will cause it to be marked as filtered, and accept will not affect its processing. 
        /// </summary>
        [JsonPropertyName("for_limited_accounts")]
        public string ForLimitedAccounts { get; set; } = string.Empty;

        /// <summary>
        /// Summary of the filtered notifications
        /// </summary>
        [JsonPropertyName("summary")]
        public Summary Summary { get; set; } = new Summary();
    }

    public class Summary
    {
        /// <summary>
        /// Number of different accounts from which the user has non-dismissed filtered notifications. Capped at 100.
        /// </summary>
        [JsonPropertyName("pending_requests_count")]
        public int PendingRequestsCount { get; set; }

        /// <summary>
        /// Number of total non-dismissed filtered notifications. May be inaccurate.
        /// </summary>
        [JsonPropertyName("pending_notifications_count")]
        public int PendingNotificationsCount { get; set; }
    }
}
