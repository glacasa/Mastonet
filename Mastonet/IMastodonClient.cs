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
        #region MastodonClient

        /// <summary>
        /// Getting instance information
        /// </summary>
        /// <returns>Returns the current Instance. Does not require authentication</returns>
        Task<Instance> GetInstance();

        /// <summary>
        /// User’s lists.
        /// </summary>
        /// <returns>Returns array of List</returns>
        Task<IEnumerable<List>> GetLists();

        /// <summary>
        /// User’s lists that a given account is part of.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns array of List</returns>
        Task<IEnumerable<List>> GetListsContainingAccount(long accountId);

        /// <summary>
        /// Accounts that are in a given list.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns array of Account</returns>
        Task<MastodonList<Account>> GetListAccounts(long listId, long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Accounts that are in a given list.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns array of Account</returns>
        Task<MastodonList<Account>> GetListAccounts(long listId, ArrayOptions options);

        /// <summary>
        /// Get a list.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>Returns List</returns>
        Task<List> GetList(long listId);

        /// <summary>
        /// Create a new list.
        /// </summary>
        /// <param name="title">The title of the list</param>
        /// <returns>The list created</returns>
        Task<List> CreateList(string title);

        /// <summary>
        /// Update a list.
        /// </summary>
        /// <param name="title">The title of the list</param>
        /// <returns>The list updated</returns>
        Task<List> UpdateList(long listId, string newTitle);

        /// <summary>
        /// Remove a list.
        /// </summary>
        /// <param name="listId"></param>
        Task DeleteList(long listId);

        /// <summary>
        /// Add accounts to a list.
        /// Only accounts already followed by the user can be added to a list.
        /// </summary>
        /// <param name="listId">List ID</param>
        /// <param name="accountIds">Array of account IDs</param>
        Task AddAccountsToList(long listId, IEnumerable<long> accountIds);

        /// <summary>
        /// Add accounts to a list.
        /// Only accounts already followed by the user can be added to a list.
        /// </summary>
        /// <param name="listId">List ID</param>
        /// <param name="accounts">Array of Accounts</param>
        Task AddAccountsToList(long listId, IEnumerable<Account> accounts);

        /// <summary>
        /// Remove accounts from a list.
        /// </summary>
        /// <param name="listId">List Id</param>
        /// <param name="accountIds">Array of Account IDs</param>
        Task RemoveAccountsFromList(long listId, IEnumerable<long> accountIds);

        /// <summary>
        /// Remove accounts from a list.
        /// </summary>
        /// <param name="listId">List Id</param>
        /// <param name="accountIds">Array of Accounts</param>
        Task RemoveAccountsFromList(long listId, IEnumerable<Account> accounts);

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="data">Media stream to be uploaded</param>
        /// <param name="fileName">Media file name (must contains extension ex: .png, .jpg, ...)</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
        /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        Task<Attachment> UploadMedia(Stream data, string fileName = "file", string description = null, AttachmentFocusData focus = null);

        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <param name="media">Media to be uploaded</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
        /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        Task<Attachment> UploadMedia(MediaDefinition media, string description = null, AttachmentFocusData focus = null);

        /// <summary>
        /// Update a media attachment. Can only be done before the media is attached to a status.
        /// </summary>
        /// <param name="mediaId">Media ID</param>
        /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
        /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
        /// <returns>Returns an Attachment that can be used when creating a status</returns>
        Task<Attachment> UpdateMedia(long mediaId, string description = null, AttachmentFocusData focus = null);

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit">Maximum number of items to get (Default 40, Max 80)</param>
        /// <param name="excludeTypes">Types to exclude</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        Task<MastodonList<Notification>> GetNotifications(long? maxId = null, long? sinceId = null, int? limit = null, NotificationType excludeTypes = 0);

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <param name="excludeTypes">Types to exclude</param>
        /// <returns>Returns a list of Notifications for the authenticated user</returns>
        Task<MastodonList<Notification>> GetNotifications(ArrayOptions options, NotificationType excludeTypes = 0);

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
        /// Delete a single notification from the server.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task DismissNotification(long notificationId);

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
        /// <param name="forward">Whether to forward to the remote admin (in case of a remote account)</param>
        /// <returns>Returns the finished Report</returns>
        Task<Report> Report(long accountId, IEnumerable<long> statusIds = null, string comment = null, bool? forward = null);

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns Results. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        Task<Results> Search(string q, bool resolve = false);

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <param name="q">The search query</param>
        /// <param name="resolve">Whether to resolve non-local accounts</param>
        /// <returns>Returns ResultsV2. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
        Task<ResultsV2> SearchV2(string q, bool resolve = false);

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <param name="q">What to search for</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <param name="resolve">Attempt WebFinger look-up (default: false)</param>
        /// <param name="following">Only who the user is following (default: false)</param>
        /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database.</returns>
        Task<List<Account>> SearchAccounts(string q, int? limit = null, bool resolve = false, bool following = false);

        /// <summary>
        /// Listing all text filters the user has configured that potentially must be applied client-side
        /// </summary>
        /// <returns>Returns an array of filters</returns>
        Task<IEnumerable<Filter>> GetFilters();

        /// <summary>
        /// Creating a new filter
        /// </summary>
        /// <param name="phrase">Keyword or phrase to filter</param>
        /// <param name="context">Filtering context. At least one context must be specified</param>
        /// <param name="irreversible">Irreversible filtering will only work in home and notifications contexts by fully dropping the records</param>
        /// <param name="wholeWord">Whether to consider word boundaries when matching</param>
        /// <param name="expiresIn">Number that indicates seconds. Filter will be expire in seconds after API processed. Leave null for no expiration</param>
        /// <returns>Returns a created filter</returns>
        Task<Filter> CreateFilter(string phrase, FilterContext context, bool irreversible = false, bool wholeWord = false, uint? expiresIn = null);

        /// <summary>
        /// Getting a text filter
        /// </summary>
        /// <param name="filterId">Filter ID</param>
        /// <returns>Returns a filter</returns>
        Task<Filter> GetFilter(long filterId);

        /// <summary>
        /// Updating a text filter
        /// </summary>
        /// <param name="filterId">Filter ID</param>
        /// <param name="phrase">A new keyword or phrase to filter, or null to keep</param>
        /// <param name="context">A new filtering context, or null to keep</param>
        /// <param name="irreversible">A new irreversible flag, or null to keep</param>
        /// <param name="wholeWord">A new whole_word flag, or null to keep</param>
        /// <param name="expiresIn">A new number that indicates seconds. Filter will be expire in seconds after API processed. Leave null to keep</param>
        /// <returns>Returns an updated filter</returns>
        Task<Filter> UpdateFilter(long filterId, string phrase = null, FilterContext? context = null, bool? irreversible = null, bool? wholeWord = null, uint? expiresIn = null);

        /// <summary>
        /// Deleting a text filter
        /// </summary>
        /// <param name="filterId"></param>
        Task DeleteFilter(long filterId);

        #endregion

        #region MastodonClient.Account

        /// <summary>
        /// Fetching an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an Account</returns>
        Task<Account> GetAccount(long accountId);

        /// <summary>
        /// Getting the current user
        /// </summary>
        /// <returns>Returns the authenticated user's Account</returns>
        Task<Account> GetCurrentUser();

        /// <summary>
        /// Updating the current user
        /// </summary>
        /// <param name="display_name">The name to display in the user's profile</param>
        /// <param name="note">A new biography for the user</param>
        /// <param name="avatar">A base64 encoded image to display as the user's avatar</param>
        /// <param name="header">A base64 encoded image to display as the user's header image</param>
        /// <param name="locked">Whether to enable follow requests</param>
        /// <param name="source_privacy">Default post privacy preference</param>
        /// <param name="source_sensitive">Whether to mark statuses as sensitive by default</param>
        /// <param name="source_language">Override language on statuses by default (ISO6391)</param>
        /// <param name="fields_attributes">Profile metadata (max. 4)</param>
        /// <returns>Returns the authenticated user's Account</returns>
        Task<Account> UpdateCredentials(string display_name = null,
            string note = null,
            MediaDefinition avatar = null,
            MediaDefinition header = null,
            bool? locked = null,
            Visibility? source_privacy = null,
            bool? source_sensitive = null,
            string source_language = null,
            IEnumerable<AccountField> fields_attributes = null);

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Returns an array of Relationships of the current user to a given account</returns>
        Task<IEnumerable<Relationship>> GetAccountRelationships(long id);

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <param name="id">Account IDs</param>
        /// <returns>Returns an array of Relationships of the current user to a list of given accounts</returns>
        Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<long> ids);

        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>        
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetAccountFollowers(long accountId, long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetAccountFollowers(long accountId, ArrayOptions options);

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetAccountFollowing(long accountId, long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetAccountFollowing(long accountId, ArrayOptions options);

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="pinned">Only return statuses that have been pinned</param>
        /// <param name="excludeReblogs">Skip statuses that are reblogs of other statuses</param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses</returns>
        Task<MastodonList<Status>> GetAccountStatuses(long accountId, long? maxId = null, long? sinceId = null, int? limit = null, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false);

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="pinned">Only return statuses that have been pinned</param>
        /// <param name="excludeReblogs">Skip statuses that are reblogs of other statuses</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses</returns>
        Task<MastodonList<Status>> GetAccountStatuses(long accountId, ArrayOptions options, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false);

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        Task<MastodonList<Account>> GetFollowRequests(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        Task<MastodonList<Account>> GetFollowRequests(ArrayOptions options);

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to authorize</param>
        Task AuthorizeRequest(long accountId);

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to reject</param>
        Task RejectRequest(long accountId);

        /// <summary>
        /// Listing accounts the user had past positive interactions with, but is not following yet
        /// </summary>
        /// <returns>Returns array of Account</returns>
        Task<IEnumerable<Account>> GetFollowSuggestions();

        /// <summary>
        /// Removing account from suggestions
        /// </summary>
        /// <param name="accountId">The account ID to remove</param>
        Task DeleteFollowSuggestion(long accountId);

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        Task<MastodonList<Status>> GetFavourites(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        Task<MastodonList<Status>> GetFavourites(ArrayOptions options);

        #endregion

        #region MastodonClient.AccountActions

        /// <summary>
        /// Following an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="reblogs">Whether the followed account’s reblogs will show up in the home timeline</param>
        /// <returns>Returns the target Account</returns>
        Task<Relationship> Follow(long accountId, bool reblogs = true);

        /// <summary>
        /// Unfollowing an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        Task<Relationship> Unfollow(long accountId);

        /// <summary>
        /// Following a remote user
        /// </summary>
        /// <param name="uri">username@domain of the person you want to follow</param>
        /// <returns>Returns the local representation of the followed account, as an Account</returns>
        Task<Account> Follow(string uri);

        /// <summary>
        /// Blocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        Task<Relationship> Block(long accountId);

        /// <summary>
        /// Unblocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        Task<Relationship> Unblock(long accountId);

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        Task<MastodonList<Account>> GetBlocks(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        Task<MastodonList<Account>> GetBlocks(ArrayOptions options);

        /// <summary>
        /// Muting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="notifications">Whether the mute will mute notifications or not</param>
        /// <returns>Returns the target Account</returns>
        Task<Relationship> Mute(long accountId, bool notifications = true);

        /// <summary>
        /// Unmuting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        Task<Relationship> Unmute(long accountId);

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        Task<MastodonList<Account>> GetMutes(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        Task<MastodonList<Account>> GetMutes(ArrayOptions options);

        /// <summary>
        /// Fetching a user's blocked domains
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of strings</returns>
        Task<MastodonList<string>> GetDomainBlocks(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Fetching a user's blocked domains
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of strings</returns>
        Task<MastodonList<string>> GetDomainBlocks(ArrayOptions options);

        /// <summary>
        /// Block a domain
        /// </summary>
        /// <param name="domain">Domain to block</param>
        Task BlockDomain(string domain);

        /// <summary>
        /// Unblock a domain
        /// </summary>
        /// <param name="domain">Domain to block</param>
        Task UnblockDomain(string domain);

        /// <summary>
        /// Getting accounts the user chose to endorse
        /// </summary>
        /// <returns>Returns an array of Accounts endorsed by the authenticated user</returns>
        Task<MastodonList<Account>> GetEndorsements();

        /// <summary>
        /// Endorsing an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the updated Relationships with the target Account</returns>
        Task<Relationship> Endorse(long accountId);

        /// <summary>
        /// Undoing endorse of an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the updated Relationships with the target Account</returns>
        Task<Relationship> Unendorse(long accountId);
        #endregion

        #region MastodonClient.Status

        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Status</returns>
        Task<Status> GetStatus(long statusId);

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Context</returns>
        Task<Context> GetStatusContext(long statusId);

        /// <summary>
        /// Getting a card associated with a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns a Card</returns>
        Task<Card> GetStatusCard(long statusId);

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetRebloggedBy(long statusId, long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetRebloggedBy(long statusId, ArrayOptions options);

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetFavouritedBy(long statusId, long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        Task<MastodonList<Account>> GetFavouritedBy(long statusId, ArrayOptions options);

        /// <summary>
        /// Posting a new status
        /// </summary>
        /// <param name="status">The text of the status</param>
        /// <param name="visibility">either "direct", "private", "unlisted" or "public"</param>
        /// <param name="replyStatusId">local ID of the status you want to reply to</param>
        /// <param name="mediaIds">array of media IDs to attach to the status (maximum 4)</param>
        /// <param name="sensitive">set this to mark the media of the status as NSFW</param>
        /// <param name="spoilerText">text to be shown as a warning before the actual content</param>
        /// <param name="scheduledAt">DateTime to schedule posting of status</param>
        /// <param name="language">Override language code of the toot (ISO 639-2)</param>
        /// <returns></returns>
        Task<Status> PostStatus(string status, Visibility? visibility = null, long? replyStatusId = null, IEnumerable<long> mediaIds = null, bool sensitive = false, string spoilerText = null, DateTime? scheduledAt = null, string language = null);

        /// <summary>
        /// Deleting a status
        /// </summary>
        /// <param name="statusId"></param>
        Task DeleteStatus(long statusId);

        /// <summary>
        /// Get scheduled statuses.
        /// </summary>
        /// <returns>Returns array of ScheduledStatus</returns>
        Task<IEnumerable<ScheduledStatus>> GetScheduledStatuses();

        /// <summary>
        /// Get scheduled status.
        /// </summary>
        /// <param name="scheduledStatusId"></param>
        /// <returns>Returns ScheduledStatus</returns>
        Task<ScheduledStatus> GetScheduledStatus(long scheduledStatusId);

        /// <summary>
        /// Update Scheduled status. Only scheduled_at can be changed. To change the content, delete it and post a new status.
        /// </summary>
        /// <param name="scheduledStatusId"></param>
        /// <param name="scheduledAt">DateTime to schedule posting of status</param>
        /// <returns>Returns ScheduledStatus</returns>
        Task<ScheduledStatus> UpdateScheduledStatus(long scheduledStatusId, DateTime? scheduledAt);

        /// <summary>
        /// Remove Scheduled status.
        /// </summary>
        /// <param name="scheduledStatusId"></param>
        Task DeleteScheduledStatus(long scheduledStatusId);

        /// <summary>
        /// Reblogging a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> Reblog(long statusId);

        /// <summary>
        /// Unreblogging a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> Unreblog(long statusId);

        /// <summary>
        /// Favouriting a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> Favourite(long statusId);

        /// <summary>
        /// Unfavouriting a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> Unfavourite(long statusId);

        /// <summary>
        /// Muting a conversation of a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> MuteConversation(long statusId);

        /// <summary>
        /// Unmuting a conversation of a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> UnmuteConversation(long statusId);

        /// <summary>
        /// Pinning a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> Pin(long statusId);

        /// <summary>
        /// Unpinning a status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns>Returns the target Status</returns>
        Task<Status> Unpin(long statusId);

        #endregion

        #region MastodonClient.Timeline

        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetHomeTimeline(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions options);

        /// <summary>
        /// Conversations (direct messages) for an account
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit">Maximum number of items to get (Default 20)</param>
        /// <returns>Returns array of Conversation</returns>
        Task<MastodonList<Conversation>> GetConversations(long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Conversations (direct messages) for an account
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns array of Conversation</returns>
        Task<MastodonList<Conversation>> GetConversations(ArrayOptions options);

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetPublicTimeline(long? maxId = null, long? sinceId = null, int? limit = null, bool local = false, bool onlyMedia = false);

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions options, bool local = false, bool onlyMedia = false);

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetTagTimeline(string hashtag, long? maxId = null, long? sinceId = null, int? limit = null, bool local = false, bool onlyMedia = false);

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false, bool onlyMedia = false);

        /// <summary>
        /// Retrieving List timeline
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit">Maximum number of items to get (Default 20)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetListTimeline(long listId, long? maxId = null, long? sinceId = null, int? limit = null);

        /// <summary>
        /// Retrieving List timeline
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        Task<MastodonList<Status>> GetListTimeline(long listId, ArrayOptions options);

        TimelineStreaming GetPublicStreaming();

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        TimelineStreaming GetPublicStreaming(string streamingApiUrl);

        TimelineStreaming GetUserStreaming();

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        TimelineStreaming GetUserStreaming(string streamingApiUrl);

        TimelineStreaming GetHashtagStreaming(string hashtag);

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        TimelineStreaming GetHashtagStreaming(string streamingApiUrl, string hashtag);

        TimelineStreaming GetDirectMessagesStreaming();

        #endregion
    }
}
