using System;
using System.Threading.Tasks;
using Nestodon.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nestodon
{
    public class NestodonClient
    {
        public string Instance { get; }

        public AppRegistration AppRegistration { get; set; }

        public Auth UserAuth { get; set; }


        #region Ctor

        public NestodonClient(string instance, AppRegistration appRegistration = null, Auth userAuth = null)
        {
            this.Instance = instance;
            this.AppRegistration = appRegistration;
            this.UserAuth = userAuth;
        }

        #endregion

        #region Http helpers

        private void AddHttpHeader(HttpClient client)
        {
            if (UserAuth != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + UserAuth.AccessToken);
            }
        }

        private async Task<string> Get(string route)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> GetObject<T>(string route)
        {
            var content = await Get(route);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> Post<T>(string route, IEnumerable<KeyValuePair<string, string>> data)
        {
            var content = await Post(route, data);
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion


        public async Task<Auth> Connect(string email, string password)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
            };



            this.UserAuth = await Post<Auth>("/oauth/token", data);
            return this.UserAuth;
        }


        #region Accounts

        /// <summary>
        /// Fetching an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an Account</returns>
        public Task<Account> GetAccount(int accountId)
        {
            return GetObject<Account>($"/api/v1/accounts/{accountId}");
        }

        /// <summary>
        /// Getting the current user
        /// </summary>
        /// <returns>Returns the authenticated user's Account</returns>
        public Task<Account> GetCurrentUser()
        {
            return GetObject<Account>($"/api/v1/accounts/verify_credentials");
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
            throw new NotImplementedException();
        }

        #endregion

        #region Apps

        public async Task<AppRegistration> RegisterApp(string appName)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("client_name", appName),
                new KeyValuePair<string, string>("redirect_uris", "urn:ietf:wg:oauth:2.0:oob"),
                new KeyValuePair<string, string>("scopes", "read"),
            };

            this.AppRegistration = await Post<AppRegistration>("/api/v1/apps", data);

            return this.AppRegistration;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unblocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Unblock(int accountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        public Task<IEnumerable<Account>> GetBlocks()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Favourites

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        public Task<IEnumerable<Status>> GetFavourites()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Follow Requests

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        public Task<IEnumerable<Account>> GetFollowRequests()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to authorize</param>
        public Task AuthorizeRequest(int accountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to reject</param>
        public Task RejectRequest(int accountId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Follows

        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetAccountFollowers(int accountId)
        {
            return GetObject<IEnumerable<Account>>($"/api/v1/accounts/{accountId}/followers");
        }

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetAccountFollowing(int accountId)
        {
            return GetObject<IEnumerable<Account>>($"/api/v1/accounts/{accountId}/following");
        }

        /// <summary>
        /// Following a remote user
        /// </summary>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an Account</returns>
        public Task<Account> Follow(string uri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Following an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Follow(int accountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unfollowing an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Unfollow(int accountId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Instances

        /// <summary>
        /// Getting instance information
        /// </summary>
        /// <returns>Returns the current Instance. Does not require authentication</returns>
        public Task<Instance> GetInstance()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unmuting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Account> Unmute(int accountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        public Task<IEnumerable<Account>> GetMutes()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        public Task<IEnumerable<Notification>> GetNotifications()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting a single notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>Returns the Notification</returns>
        public Task<Notification> GetNotification(int notificationId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes all notifications from the Mastodon server for the authenticated user
        /// </summary>
        /// <returns></returns>
        public Task ClearNotifications()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Reports

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        public Task<IEnumerable<Report>> GetReports()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reporting a user
        /// </summary>
        /// <param name="accountIs">The ID of the account to report</param>
        /// <param name="statusIds">The IDs of statuses to report</param>
        /// <param name="comment">A comment to associate with the report</param>
        /// <returns>Returns the finished Report</returns>
        public Task<Report> Report(int accountIs, IEnumerable<int> statusIds, string comment)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Search

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <param name="q">What to search for</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database</returns>
        public Task<IEnumerable<Account>> SearchAccounts(string q, int? limit = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns Results. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        public Task<Results> Search(string q, bool resolve)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Statuses

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <returns>Returns an array of Statuses</returns>
        public Task<IEnumerable<Status>> GetAccountStatuses(int accountId, bool onlyMedia = false, bool excludeReplies = false)
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

            return GetObject<IEnumerable<Status>>(url + queryParams);
        }

        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Status</returns>
        public Task<Status> GetStatus(int statusId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Context</returns>
        public Task<Context> GetStatusContext(int statusId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting a card associated with a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Card</returns>
        public Task<Card> GetStatusCard(int statusId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetRebloggedBy(int statusId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<IEnumerable<Account>> GetFavouritedBy(int statusId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Posting a new status
        /// </summary>
        /// <param name="status">The text of the status</param>
        /// <param name="replyStatusId">local ID of the status you want to reply to</param>
        /// <param name="mediaIds">array of media IDs to attach to the status (maximum 4)</param>
        /// <param name="sensitive">set this to mark the media of the status as NSFW</param>
        /// <param name="spoilerText">text to be shown as a warning before the actual content</param>
        /// <param name="visibility">either "direct", "private", "unlisted" or "public"</param>
        /// <returns></returns>
        public Task<Status> PostStatus(string status, int? replyStatusId = null, IEnumerable<int> mediaIds = null, bool sensitive = false, string spoilerText = null, string visibility = null)
        {            
            throw new NotImplementedException();
        }

        #endregion

    }
}
