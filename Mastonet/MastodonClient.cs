using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mastonet
{
    public partial class MastodonClient : BaseHttpClient, IMastodonClient
    {
        #region Ctor

        public MastodonClient(AppRegistration appRegistration, Auth accessToken)
        {
            this.Instance = appRegistration.Instance;
            this.AppRegistration = appRegistration;
            this.AuthToken = accessToken;

#if NETSTANDARD2_0
            this.instanceGetter = new Lazy<Task<Instance>>(this.GetInstance);
#endif
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

        #region Lists

        /// <summary>
        /// User’s lists.
        /// </summary>
        /// <returns>Returns array of List</returns>
        public Task<IEnumerable<List>> GetLists()
        {
            return this.Get<IEnumerable<List>>("/api/v1/lists");
        }

        public Task<List> GetList(long listId)
        {
            return this.Get<List>("/api/v1/lists/" + listId);
        }

        /// <summary>
        /// User’s lists that a given account is part of.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns array of List</returns>
        public Task<IEnumerable<List>> GetListsContainingAccount(long accountId)
        {
            return this.Get<IEnumerable<List>>($"/api/v1/accounts/{accountId}/lists");
        }

        /// <summary>
        /// Accounts that are in a given list.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>Returns array of Account</returns>
        public Task<IEnumerable<Account>> GetListAccounts(long listId, int? limit = null)
        {
            return this.Get<IEnumerable<Account>>($"/api/v1/lists/{listId}/accounts");
        }

        /// <summary>
        /// Create a new list.
        /// </summary>
        /// <param name="title">The title of the list</param>
        /// <returns>The list created</returns>
        public Task<List> CreateList(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("The title is required", nameof(title));
            }

            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("title", title),
            };

            return this.Post<List>("/api/v1/lists", data);
        }

        /// <summary>
        /// Create a new list.
        /// </summary>
        /// <param name="title">The title of the list</param>
        /// <returns>The list created</returns>
        public Task<List> UpdateList(long listId, string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
            {
                throw new ArgumentException("The title is required", nameof(newTitle));
            }

            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("title", newTitle),
            };

            return this.Put<List>("/api/v1/lists/" + listId, data);
        }

        public Task DeleteList(long listId)
        {
            return this.Delete("/api/v1/lists/" + listId);
        }

        public Task AddAccountsToList(long listId, IEnumerable<long> accountIds)
        {
            if (accountIds == null || !accountIds.Any())
            {
                throw new ArgumentException("Accounts are required", nameof(accountIds));
            }

            var data = accountIds.Select(id => new KeyValuePair<string, string>("account_ids[]", id.ToString()));

            return this.Post($"/api/v1/lists/{listId}/accounts", data);
        }

        public Task AddAccountsToList(long listId, IEnumerable<Account> accounts)
        {
            if (accounts == null || !accounts.Any())
            {
                throw new ArgumentException("Accounts are required", nameof(accounts));
            }

            var data = accounts.Select(act => new KeyValuePair<string, string>("account_ids[]", act.Id.ToString()));

            return this.Post($"/api/v1/lists/{listId}/accounts", data);
        }

        private Task RemoveAccountsFromList(long listId, IEnumerable<long> accountIds)
        {
            throw new NotImplementedException();
            //if (accountIds == null || !accountIds.Any())
            //{
            //    throw new ArgumentException("Accounts are required", nameof(accountIds));
            //}

            //var data = accountIds.Select(id => new KeyValuePair<string, string>("account_ids[]", id.ToString()));

            //TODO : Delete with data
            //return this.Delete($"/api/v1/lists/{listId}/accounts", data);
        }

        private Task RemoveAccountsFromList(long listId, IEnumerable<Account> accounts)
        {
            throw new NotImplementedException();
            //if (accounts == null || !accounts.Any())
            //{
            //    throw new ArgumentException("Accounts are required", nameof(accounts));
            //}

            //var data = accounts.Select(act => new KeyValuePair<string, string>("account_ids[]", act.Id.ToString()));

            //TODO : Delete with data
            //return this.Delete($"/api/v1/lists/{listId}/accounts", data);
        }

        #endregion

        #region Media

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="data">Media stream to be uploaded</param>
        /// <param name="fileName">Media file name (must contains extension ex: .png, .jpg, ...)</param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        public Task<Attachment> UploadMedia(Stream data, string fileName = "file")
        {
            return UploadMedia(new MediaDefinition(data, fileName));
        }

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="media">Media to be uploaded</param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        public Task<Attachment> UploadMedia(MediaDefinition media)
        {
            media.ParamName = "file";
            var list = new List<MediaDefinition>() { media };
            return this.Post<Attachment>("/api/v1/media", null, list);
        }

        #endregion

        #region Emojis

        /// <summary>
        /// Custom emojis that are available on the server.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Emoji>> GetCustomEmojis()
        {
            return this.Get<IEnumerable<Emoji>>("/api/v1/custom_emojis");
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        public Task<MastodonList<Notification>> GetNotifications(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetNotifications(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        public Task<MastodonList<Notification>> GetNotifications(ArrayOptions options)
        {
            var url = "/api/v1/notifications";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Notification>(url);
        }

        /// <summary>
        /// Getting a single notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>Returns the Notification</returns>
        public Task<Notification> GetNotification(long notificationId)
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
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        public Task<MastodonList<Report>> GetReports(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetReports(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        public Task<MastodonList<Report>> GetReports(ArrayOptions options)
        {
            var url = "/api/v1/reports";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Report>(url);
        }

        /// <summary>
        /// Reporting a user
        /// </summary>
        /// <param name="accountId">The ID of the account to report</param>
        /// <param name="statusIds">The IDs of statuses to report</param>
        /// <param name="comment">A comment to associate with the report</param>
        /// <returns>Returns the finished Report</returns>
        public Task<Report> Report(long accountId, IEnumerable<long> statusIds, string comment)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("account_id", accountId.ToString()),
                new KeyValuePair<string, string>("comment", comment),
            };
            foreach (var statusId in statusIds)
            {
                data.Add(new KeyValuePair<string, string>("status_ids[]", statusId.ToString()));
            }

            return Post<Report>("/api/v1/reports", data);
        }

        #endregion

        #region Search
        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns Results. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        public Task<Results> Search(string q, bool resolve = false)
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

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <param name="q">What to search for</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database.</returns>
        public Task<List<Account>> SearchAccounts(string q, int? limit = null)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Task.FromResult(new List<Account>());
            }

            string url = "/api/v1/accounts/search?q=" + Uri.EscapeUriString(q);
            if (limit.HasValue)
            {
                url += "&limit=" + limit.Value;
            }

            return Get<List<Account>>(url);
        }

        [Obsolete("maxId ans sinceId are not used for account search. Use SearchAccounts(string q, int? limit) instead")]
        public Task<List<Account>> SearchAccounts(string q, long? maxId, long? sinceId, int? limit = null)
        {
            return SearchAccounts(q, limit);
        }

        [Obsolete("options are not used for account search. Use SearchAccounts(string q, int? limit) instead")]
        public Task<List<Account>> SearchAccounts(string q, ArrayOptions options)
        {
            return SearchAccounts(q, options?.Limit);
        }
        #endregion

    }
}
