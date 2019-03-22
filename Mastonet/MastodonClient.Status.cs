using System;
using System.Threading.Tasks;
using Mastonet.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace Mastonet
{
    partial class MastodonClient
    {
        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Status</returns>
        public Task<Status> GetStatus(long statusId)
        {
            return Get<Status>($"/api/v1/statuses/{statusId}");
        }

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Context</returns>
        public Task<Context> GetStatusContext(long statusId)
        {
            return Get<Context>($"/api/v1/statuses/{statusId}/context");
        }

        /// <summary>
        /// Getting a card associated with a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Card</returns>
        public Task<Card> GetStatusCard(long statusId)
        {
            return Get<Card>($"/api/v1/statuses/{statusId}/card");
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetRebloggedBy(long statusId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetRebloggedBy(statusId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetRebloggedBy(long statusId, ArrayOptions options)
        {
            var url = $"/api/v1/statuses/{statusId}/reblogged_by";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetFavouritedBy(long statusId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetFavouritedBy(statusId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetFavouritedBy(long statusId, ArrayOptions options)
        {
            var url = $"/api/v1/statuses/{statusId}/favourited_by";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Account>(url);
        }
        /// <summary>
        /// Posting a new status
        /// </summary>
        /// <param name="status">The text of the status</param>
        /// <param name="visibility">either "direct", "private", "unlisted" or "public"</param>
        /// <param name="replyStatusId">local ID of the status you want to reply to</param>
        /// <param name="mediaIds">array of media IDs to attach to the status (maximum 4)</param>
        /// <param name="sensitive">set this to mark the media of the status as NSFW</param>
        /// <param name="spoilerText">text to be shown as a warning before the actual content</param>
        /// <returns></returns>
        public Task<Status> PostStatus(string status, Visibility visibility, long? replyStatusId = null, IEnumerable<long> mediaIds = null, bool sensitive = false, string spoilerText = null)
        {
			if (string.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }

            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("status", status),
            };

            if (replyStatusId.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("in_reply_to_id", replyStatusId.Value.ToString()));
            }
            if (mediaIds != null && mediaIds.Any())
            {
                foreach (var mediaId in mediaIds)
                    data.Add(new KeyValuePair<string, string>("media_ids[]", mediaId.ToString()));
            }
            if (sensitive)
            {
                data.Add(new KeyValuePair<string, string>("sensitive", "true"));
            }
            if (!string.IsNullOrEmpty(spoilerText))
            {
                data.Add(new KeyValuePair<string, string>("spoiler_text", spoilerText));
            }

            data.Add(new KeyValuePair<string, string>("visibility", visibility.ToString().ToLower()));

            return Post<Status>("/api/v1/statuses", data);
        }
		
        /// <summary>
        /// Deleting a status
        /// </summary>
        /// <param name="statusId"></param>
        public Task DeleteStatus(long statusId)
        {
            return Delete($"/api/v1/statuses/{statusId}");
        }

        /// <summary>
        /// Reblogging a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Reblog(long statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/reblog");
        }

        /// <summary>
        /// Unreblogging a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Unreblog(long statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/unreblog");
        }

        /// <summary>
        /// Favouriting a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Favourite(long statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/favourite");
        }

        /// <summary>
        /// Unfavouriting a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Unfavourite(long statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/unfavourite");
        }

        /// <summary>
        /// Muting a conversation of a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> MuteConversation(long statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/mute");
        }

        /// <summary>
        /// Unmuting a conversation of a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> UnmuteConversation(long statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/unmute");
        }


    }
}
