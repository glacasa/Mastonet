using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mastonet
{
    public interface IMastodonClient : IBaseHttpClient
    {
        /// <summary>
        /// Getting instance information
        /// </summary>
        /// <returns>Returns the current Instance. Does not require authentication</returns>
        Task<Instance> GetInstance();

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="data">Media stream to be uploaded</param>
        /// <param name="fileName">Media file name (must contains extension ex: .png, .jpg, ...)</param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        Task<Attachment> UploadMedia(Stream data, string fileName = "file");

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        Task<MastodonList<Notification>> GetNotifications(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        Task<MastodonList<Notification>> GetNotifications(ArrayOptions options);

        /// <summary>
        /// Getting a single notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns>Returns the Notification</returns>
        Task<Notification> GetNotification(long notificationId);

        /// <summary>
        /// Deletes all notifications from the Mastodon server for the authenticated user
        /// </summary>
        /// <returns></returns>
        Task ClearNotifications();

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        Task<MastodonList<Report>> GetReports(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns a list of Reports made by the authenticated user</returns>
        Task<MastodonList<Report>> GetReports(ArrayOptions options);

        /// <summary>
        /// Reporting a user
        /// </summary>
        /// <param name="accountId">The ID of the account to report</param>
        /// <param name="statusIds">The IDs of statuses to report</param>
        /// <param name="comment">A comment to associate with the report</param>
        /// <returns>Returns the finished Report</returns>
        Task<Report> Report(long accountId, IEnumerable<long> statusIds, string comment);

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns Results. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        Task<Results> Search(string q, bool resolve = false);

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <param name="q">What to search for</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database.</returns>
        Task<List<Account>> SearchAccounts(string q, int? limit = null);

        [Obsolete("maxId ans sinceId are not used for account search. Use SearchAccounts(string q, int? limit) instead")]
        Task<List<Account>> SearchAccounts(string q, long? maxId, long? sinceId, int? limit = null);

        [Obsolete("options are not used for account search. Use SearchAccounts(string q, int? limit) instead")]
        Task<List<Account>> SearchAccounts(string q, ArrayOptions options);
    }
}
