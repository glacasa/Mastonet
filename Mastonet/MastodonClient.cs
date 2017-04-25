using System;
using System.Threading.Tasks;
using Mastonet.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Mastonet
{
    public partial class MastodonClient : BaseHttpClient
    {
        #region Ctor

        public MastodonClient(AppRegistration appRegistration, Auth accessToken)
        {
            this.Instance = appRegistration.Instance;
            this.AppRegistration = appRegistration;
            this.AuthToken = accessToken;
        }

        #endregion
        
        #region Accounts

        /// <summary>
        /// Fetching an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an Account</returns>
        public Task<Account> GetAccount(int accountId)
        {
            return Get<Account>($"/api/v1/accounts/{accountId}");
        }

        /// <summary>
        /// Getting the current user
        /// </summary>
        /// <returns>Returns the authenticated user's Account</returns>
        public Task<Account> GetCurrentUser()
        {
            return Get<Account>($"/api/v1/accounts/verify_credentials");
        }

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Returns an array of Relationships of the current user to a given account</returns>
        public Task<IEnumerable<Relationship>> GetAccountRelationships(int id)
        {
            return GetAccountRelationships(new int[] { id });
        }

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <param name="id">Account IDs</param>
        /// <returns>Returns an array of Relationships of the current user to a list of given accounts</returns>
        public Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<int> ids)
        {
            return Get<IEnumerable<Relationship>>("/api/v1/accounts/relationships");
        }

        /// <summary>
        /// Updating the current user
        /// </summary>
        /// <param name="display_name">The name to display in the user's profile</param>
        /// <param name="note">A new biography for the user</param>
        /// <param name="avatar">A base64 encoded image to display as the user's avatar</param>
        /// <param name="header">A base64 encoded image to display as the user's header image</param>
        /// <returns>Returns an array of Relationships of the current user to a list of given accounts</returns>
        public Task<Account> UpdateCredentials(string display_name = null, string note = null, string avatar = null, string header = null)
        {
            var data = new List<KeyValuePair<string, string>>();

            if (display_name != null)
            {
                data.Add(new KeyValuePair<string, string>("display_name", display_name));
            }
            if (note != null)
            {
                data.Add(new KeyValuePair<string, string>("note", note));
            }
            if (avatar != null)
            {
                data.Add(new KeyValuePair<string, string>("avatar", avatar));
            }
            if (header != null)
            {
                data.Add(new KeyValuePair<string, string>("header", header));
            }

            return Patch<Account>($"/api/v1/accounts/update_credentials", data);
        }

        #endregion

        #region Blocks

        /// <summary>
        /// Blocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Block(int accountId)
        {
            return Get<Account>($"/api/v1/accounts/{accountId}/block");
        }

        /// <summary>
        /// Unblocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Unblock(int accountId)
        {
            return Get<Account>($"/api/v1/accounts/{accountId}/unblock");
        }

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        public Task<IEnumerable<Account>> GetBlocks(int? maxId = null, int? sinceId = null)
        {
            return GetBlocks(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        public Task<IEnumerable<Account>> GetBlocks(ArrayOptions options)
        {
            var url = "/api/v1/blocks";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Account>>(url);
        }

        #endregion

        #region Favourites

        /// <summary>
        /// Favouriting a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Favourite(int statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/favourite");
        }

        /// <summary>
        /// Unfavouriting a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Unfavourite(int statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/unfavourite");
        }

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        public Task<IEnumerable<Status>> GetFavourites(int? maxId = null, int? sinceId = null)
        {
            return GetFavourites(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        public Task<IEnumerable<Status>> GetFavourites(ArrayOptions options)
        {
            var url = "/api/v1/favourites";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Status>>(url);
        }

        #endregion

        #region Follow Requests

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        public Task<IEnumerable<Account>> GetFollowRequests(int? maxId = null, int? sinceId = null)
        {
            return GetFollowRequests(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        public Task<IEnumerable<Account>> GetFollowRequests(ArrayOptions options)
        {
            var url = "/api/v1/follow_requests";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return this.Get<IEnumerable<Account>>(url);
        }

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to authorize</param>
        public Task AuthorizeRequest(int accountId)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", accountId.ToString())
            };
            return this.Post("/api/v1/follow_requests/authorize");
        }

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to reject</param>
        public Task RejectRequest(int accountId)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", accountId.ToString())
            };
            return this.Post("/api/v1/follow_requests/reject");
        }

        #endregion

        #region Follows

        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>        
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetAccountFollowers(int accountId, int? maxId = null, int? sinceId = null)
        {
            return GetAccountFollowers(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }


        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetAccountFollowers(int accountId, ArrayOptions options)
        {
            var url = $"/api/v1/accounts/{accountId}/followers";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Account>>(url);
        }

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetAccountFollowing(int accountId, int? maxId = null, int? sinceId = null)
        {
            return GetAccountFollowing(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetAccountFollowing(int accountId, ArrayOptions options)
        {
            var url = $"/api/v1/accounts/{accountId}/following";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Account>>(url);
        }

        /// <summary>
        /// Following a remote user
        /// </summary>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an Account</returns>
        public Task<Account> Follow(string uri)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("uri", uri)
            };
            return this.Post<Account>($"/api/v1/follows", data);
        }

        /// <summary>
        /// Following an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Follow(int accountId)
        {
            return this.Post<Account>($"/api/v1/accounts/{accountId}/follow");
        }

        /// <summary>
        /// Unfollowing an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Unfollow(int accountId)
        {
            return this.Post<Account>($"/api/v1/accounts/{accountId}/unfollow");
        }

        #endregion

        #region Instances

        /// <summary>
        /// Getting instance information
        /// </summary>
        /// <returns>Returns the current Instance. Does not require authentication</returns>
        public Task<Instance> GetInstance()
        {
            return this.Get<Instance>("/api/v1/instance");
        }

        #endregion

        #region Media

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="file">Media to be uploaded</param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        public Task<Attachment> UploadMedia(object file)
        {
            throw new NotImplementedException();

            // TODO : upload attachment
            // return this.Post<Attachment>("/api/v1/media");
        }

        #endregion

        #region Mutes

        /// <summary>
        /// Muting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Mute(int accountId)
        {
            return Get<Account>($"/api/v1/accounts/{accountId}/mute");
        }

        /// <summary>
        /// Unmuting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Unmute(int accountId)
        {
            return Get<Account>($"/api/v1/accounts/{accountId}/unmute");
        }

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        public Task<IEnumerable<Account>> GetMutes(int? maxId = null, int? sinceId = null)
        {
            return GetMutes(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        public Task<IEnumerable<Account>> GetMutes(ArrayOptions options)
        {
            var url = "/api/v1/mutes";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Account>>(url);
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        public Task<IEnumerable<Notification>> GetNotifications(int? maxId = null, int? sinceId = null)
        {
            return GetNotifications(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        public Task<IEnumerable<Notification>> GetNotifications(ArrayOptions options)
        {
            var url = "/api/v1/notifications";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Notification>>(url);
        }

        /// <summary>
        /// Getting a single notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>Returns the Notification</returns>
        public Task<Notification> GetNotification(int notificationId)
        {
            return Get<Notification>($"/api/v1/notifications/{notificationId}");
        }

        /// <summary>
        /// Deletes all notifications from the Mastodon server for the authenticated user
        /// </summary>
        /// <returns></returns>
        public Task ClearNotifications()
        {
            return Post("/api/v1/notifications/clear");
        }

        #endregion

        #region Reports

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        public Task<IEnumerable<Report>> GetReports(int? maxId = null, int? sinceId = null)
        {
            return GetReports(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        public Task<IEnumerable<Report>> GetReports(ArrayOptions options)
        {
            var url = "/api/v1/reports";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Report>>(url);
        }

        /// <summary>
        /// Reporting a user
        /// </summary>
        /// <param name="accountId">The ID of the account to report</param>
        /// <param name="statusIds">The IDs of statuses to report</param>
        /// <param name="comment">A comment to associate with the report</param>
        /// <returns>Returns the finished Report</returns>
        public Task<Report> Report(int accountId, IEnumerable<int> statusIds, string comment)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("account_id", accountId.ToString()),
                new KeyValuePair<string, string>("status_ids", JsonConvert.SerializeObject(statusIds)),
                new KeyValuePair<string, string>("comment", comment),
            };

            return Post<Report>("/api/v1/reports", data);
        }

        #endregion

        #region Search

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <param name="q">What to search for</param>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database</returns>
        public Task<IEnumerable<Account>> SearchAccounts(string q, int? maxId = null, int? sinceId = null, int? limit = null)
        {
            return SearchAccounts(q, new ArrayOptions() { MaxId = maxId, SinceId = sinceId }, limit);
        }

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <param name="q">What to search for</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database</returns>
        public Task<IEnumerable<Account>> SearchAccounts(string q, ArrayOptions options, int? limit = null)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Task.FromResult(Enumerable.Empty<Account>());
            }

            string url = "/api/v1/accounts/search?q=" + Uri.EscapeUriString(q);
            if (limit.HasValue)
            {
                url += "&limit=" + limit.Value;
            }
            if (options != null)
            {
                url += "&" + options.ToQueryString();
            }

            return Get<IEnumerable<Account>>(url);
        }

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns Results. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        public Task<Results> Search(string q, bool resolve)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Task.FromResult(new Results());
            }

            string url = "/api/v1/search?q=" + Uri.EscapeUriString(q);
            if (resolve)
            {
                url += "&resolve=true";
            }

            return Get<Results>(url);
        }

        #endregion

        #region Statuses

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Statuses</returns>
        public Task<IEnumerable<Status>> GetAccountStatuses(int accountId, int? maxId = null, int? sinceId = null, bool onlyMedia = false, bool excludeReplies = false)
        {
            return GetAccountStatuses(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId }, onlyMedia, excludeReplies);
        }

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses</returns>
        public Task<IEnumerable<Status>> GetAccountStatuses(int accountId, ArrayOptions options, bool onlyMedia = false, bool excludeReplies = false)
        {
            var url = $"/api/v1/accounts/{accountId}/statuses";

            string queryParams = "";
            if (onlyMedia)
            {
                queryParams = "?only_media=true";
            }
            if (excludeReplies)
            {
                if (queryParams != "")
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }
                queryParams += "exclude_replies=true";
            }
            if (options != null)
            {
                if (queryParams != "")
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }
                queryParams += options.ToQueryString();
            }

            return Get<IEnumerable<Status>>(url + queryParams);
        }

        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Status</returns>
        public Task<Status> GetStatus(int statusId)
        {
            return Get<Status>($"/api/v1/statuses/{statusId}");
        }

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Context</returns>
        public Task<Context> GetStatusContext(int statusId)
        {
            return Get<Context>($"/api/v1/statuses/{statusId}/context");
        }

        /// <summary>
        /// Getting a card associated with a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Card</returns>
        public Task<Card> GetStatusCard(int statusId)
        {
            return Get<Card>($"/api/v1/statuses/{statusId}/card");
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetRebloggedBy(int statusId, int? maxId = null, int? sinceId = null)
        {
            return GetRebloggedBy(statusId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetRebloggedBy(int statusId, ArrayOptions options)
        {
            var url = $"/api/v1/statuses/{statusId}/reblogged_by";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Account>>(url);
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetFavouritedBy(int statusId, int? maxId = null, int? sinceId = null)
        {
            return GetFavouritedBy(statusId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetFavouritedBy(int statusId, ArrayOptions options)
        {
            var url = $"/api/v1/statuses/{statusId}/favourited_by";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Account>>(url);
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
        public Task<Status> PostStatus(string status, Visibility visibility, int? replyStatusId = null, IEnumerable<int> mediaIds = null, bool sensitive = false, string spoilerText = null)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("status", status),
            };

            if (replyStatusId.HasValue)
            {
                data.Add(new KeyValuePair<string, string>("in_reply_to_id", replyStatusId.Value.ToString()));
            }
            if (mediaIds != null && mediaIds.Any())
            {
                var mediaJson = JsonConvert.SerializeObject(mediaIds);
                data.Add(new KeyValuePair<string, string>("media_ids", mediaJson));
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
        public Task DeleteStatus(int statusId)
        {
            return Delete($"/api/v1/statuses/{statusId}");
        }

        /// <summary>
        /// Reblogging a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Reblog(int statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/reblog");
        }

        /// <summary>
        /// Unreblogging a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        public Task<Status> Unreblog(int statusId)
        {
            return Post<Status>($"/api/v1/statuses/{statusId}/unreblog");
        }
        #endregion

        #region Timelines

        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<IEnumerable<Status>> GetHomeTimeline(int? maxId = null, int? sinceId = null)
        {
            return GetHomeTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId });
        }

        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<IEnumerable<Status>> GetHomeTimeline(ArrayOptions options)
        {
            string url = "/api/v1/timelines/home";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return Get<IEnumerable<Status>>(url);
        }

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<IEnumerable<Status>> GetPublicTimeline(int? maxId = null, int? sinceId = null, bool local = false)
        {
            return GetPublicTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId }, local);
        }

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<IEnumerable<Status>> GetPublicTimeline(ArrayOptions options, bool local = false)
        {
            string url = "/api/v1/timelines/public";

            var queryParams = "";
            if (local)
            {
                queryParams += "?local=true";
            }
            if (options != null)
            {
                if (queryParams != "")
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }
                queryParams += options.ToQueryString();
            }

            return Get<IEnumerable<Status>>(url + queryParams);
        }

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="maxId">Define the last items to get</param>
        /// <param name="sinceId">Define the first items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<IEnumerable<Status>> GetTagTimeline(string hashtag, int? maxId = null, int? sinceId = null, bool local = false)
        {
            return GetTagTimeline(hashtag, new ArrayOptions() { MaxId = maxId, SinceId = sinceId }, local);
        }

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<IEnumerable<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false)
        {
            string url = "/api/v1/timelines/tag/" + hashtag;

            var queryParams = "";
            if (local)
            {
                queryParams += "?local=true";
            }
            if (options != null)
            {
                if (queryParams != "")
                {
                    queryParams += "&";
                }
                else
                {
                    queryParams += "?";
                }
                queryParams += options.ToQueryString();
            }

            return Get<IEnumerable<Status>>(url + queryParams);
        }

        #endregion

        #region Streaming

        private string StreamingApiUrl
        {
            get
            {
                // mstdn.jp uses a different url for its streaming API, althoug it's supposed to be a dev feature.
                // if other instances have the same problem, they will be added there, until the API is updated to
                // allow us get the correct url
                switch (this.Instance)
                {
                    case "mstdn.jp":
                        return "streaming.mstdn.jp";
                    default:
                        return Instance;
                }
            }
        }

        public TimelineStreaming GetPublicStreaming()
        {
            string url = "https://" + StreamingApiUrl + "/api/v1/streaming/public";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        public TimelineStreaming GetPublicStreaming(string streamingApiUrl)
        {
            string url = "https://" + streamingApiUrl + "/api/v1/streaming/public";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        public TimelineStreaming GetUserStreaming()
        {
            string url = "https://" + StreamingApiUrl + "/api/v1/streaming/user";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        public TimelineStreaming GetUserStreaming(string streamingApiUrl)
        {
            string url = "https://" + streamingApiUrl + "/api/v1/streaming/user";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        public TimelineStreaming GetHashtagStreaming(string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", "hashtag");
            }

            string url = "https://" + StreamingApiUrl + "/api/v1/streaming/hashtag?tag=" + hashtag;

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        public TimelineStreaming GetHashtagStreaming(string streamingApiUrl, string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", "hashtag");
            }

            string url = "https://" + streamingApiUrl + "/api/v1/streaming/hashtag?tag=" + hashtag;

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        #endregion

    }
}
