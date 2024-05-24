using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mastonet;

public interface IMastodonClient
{
    string Instance { get; }

    #region MastodonClient

    /// <summary>
    /// Getting instance information
    /// </summary>
    /// <returns>Returns the current Instance. Does not require authentication</returns>
    [Obsolete("This method is deprecated on Mastodon v4. Use GetInstanceV2() instead.")]
    Task<Instance> GetInstance();

    /// <summary>
    /// Getting instance information
    /// </summary>
    /// <returns>Returns the current Instance. Does not require authentication</returns>
    Task<InstanceV2> GetInstanceV2();

    /// <summary>
    /// List of connected domains
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string>> GetInstancePeers();

    /// <summary>
    /// Weekly activity
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Activity>> GetInstanceActivity();

    /// <summary>
    /// Tags that are being used more frequently within the past week.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Tag>> GetTrendingTags();

    /// <summary>
    /// A directory of profiles that your website is aware of.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <param name="order"></param>
    /// <param name="local"></param>
    /// <returns></returns>
    Task<IEnumerable<Account>> GetDirectory(int? offset, int? limit, DirectoryOrder? order, bool? local);

    /// <summary>
    /// Get all announcements set by administration
    /// </summary>
    /// <param name="withDismissed">If true, response will include announcements dismissed by the user</param>
    /// <returns></returns>
    Task<IEnumerable<Announcement>> GetAnnouncements(bool withDismissed = false);

    /// <summary>
    /// Allows a user to mark the announcement as read
    /// </summary>
    /// <param name="id"></param>
    Task DismissAnnouncement(string id);

    /// <summary>
    /// React to an announcement with an emoji
    /// </summary>
    /// <param name="id">Local ID of an announcement in the database</param>
    /// <param name="emoji">Unicode emoji, or shortcode of custom emoji</param>
    Task AddReactionToAnnouncement(string id, string emoji);

    /// <summary>
    /// Undo a react emoji to an announcement
    /// </summary>
    /// <param name="id">Local ID of an announcement in the database</param>
    /// <param name="emoji">Unicode emoji, or shortcode of custom emoji</param>
    /// <returns></returns>
    Task RemoveReactionFromAnnouncement(string id, string emoji);

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
    Task<IEnumerable<List>> GetListsContainingAccount(string accountId);

    /// <summary>
    /// Accounts that are in a given list.
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns array of Account</returns>
    Task<MastodonList<Account>> GetListAccounts(string listId, ArrayOptions? options = null);

    /// <summary>
    /// Get a list.
    /// </summary>
    /// <param name="listId"></param>
    /// <returns>Returns List</returns>
    Task<List> GetList(string listId);

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
    /// <param name="exclusive">Whether members of this list need to get removed from the “Home” feed</param>
    /// <param name="replies_policy">One of followed, list, or none. Defaults to list.</param>
    /// <returns>The list updated</returns>
    Task<List> UpdateList(string listId, string newTitle, bool exclusive, string replies_policy);

    /// <summary>
    /// Remove a list.
    /// </summary>
    /// <param name="listId"></param>
    Task DeleteList(string listId);

    /// <summary>
    /// Add accounts to a list.
    /// Only accounts already followed by the user can be added to a list.
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="accountIds">Array of account IDs</param>
    Task AddAccountsToList(string listId, IEnumerable<string> accountIds);

    /// <summary>
    /// Add accounts to a list.
    /// Only accounts already followed by the user can be added to a list.
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="accounts">Array of Accounts</param>
    Task AddAccountsToList(string listId, IEnumerable<Account> accounts);

    /// <summary>
    /// Remove accounts from a list.
    /// </summary>
    /// <param name="listId">List Id</param>
    /// <param name="accountIds">Array of Account IDs</param>
    Task RemoveAccountsFromList(string listId, IEnumerable<string> accountIds);

    /// <summary>
    /// Remove accounts from a list.
    /// </summary>
    /// <param name="listId">List Id</param>
    /// <param name="accountIds">Array of Accounts</param>
    Task RemoveAccountsFromList(string listId, IEnumerable<Account> accounts);

    /// <summary>
    /// Uploading a media attachment
    /// </summary>
    /// <param name="data">Media stream to be uploaded</param>
    /// <param name="fileName">Media file name (must contains extension ex: .png, .jpg, ...)</param>
    /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
    /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
    /// <returns>Returns an Attachment that can be used when creating a status</returns>
    Task<Attachment> UploadMedia(Stream data, string fileName = "file", string? description = null, AttachmentFocusData? focus = null);

    /// <summary>
    /// Uploading a media attachment
    /// </summary>
    /// <param name="media">Media to be uploaded</param>
    /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
    /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
    /// <returns>Returns an Attachment that can be used when creating a status</returns>
    Task<Attachment> UploadMedia(MediaDefinition media, string? description = null, AttachmentFocusData? focus = null);

    /// <summary>
    /// Update a media attachment. Can only be done before the media is attached to a status.
    /// </summary>
    /// <param name="mediaId">Media ID</param>
    /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
    /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
    /// <returns>Returns an Attachment that can be used when creating a status</returns>
    Task<Attachment> UpdateMedia(string mediaId, string? description = null, AttachmentFocusData? focus = null);

    /// <summary>
    /// Fetching a user's notifications
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <param name="excludeTypes">Types to exclude</param>
    /// <returns>Returns a list of Notifications for the authenticated user</returns>
    Task<MastodonList<Notification>> GetNotifications(ArrayOptions? options = null, NotificationType excludeTypes = NotificationType.None);

    /// <summary>
    /// Getting a single notification
    /// </summary>
    /// <param name="notificationId"></param>
    /// <returns>Returns the Notification</returns>
    Task<Notification> GetNotification(string notificationId);

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
    Task DismissNotification(string notificationId);


    /// <summary>
    /// Fetching a user's reports
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns a list of Reports made by the authenticated user</returns>
    Task<MastodonList<Report>> GetReports(ArrayOptions? options = null);

    /// <summary>
    /// Reporting a user
    /// </summary>
    /// <param name="accountId">The ID of the account to report</param>
    /// <param name="statusIds">The IDs of statuses to report</param>
    /// <param name="comment">A comment to associate with the report</param>
    /// <param name="forward">Whether to forward to the remote admin (in case of a remote account)</param>
    /// <returns>Returns the finished Report</returns>
    Task<Report> Report(string accountId, IEnumerable<string>? statusIds = null, string? comment = null, bool? forward = null);

    /// <summary>
    /// Searching for content
    /// </summary>
    /// <param name="q">The search query</param>
    /// <param name="resolve">Whether to resolve non-local accounts</param>
    /// <returns>Returns ResultsV2. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
    Task<SearchResults> Search(string q, bool resolveNonLocalAccouns = false);

    /// <summary>
    /// Searching for accounts
    /// </summary>
    /// <param name="q">What to search for</param>
    /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
    /// <param name="resolve">Attempt WebFinger look-up (default: false)</param>
    /// <param name="following">Only who the user is following (default: false)</param>
    /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database.</returns>
    Task<List<Account>> SearchAccounts(string q, int? limit = null, bool resolveNonLocalAccouns = false, bool onlyFollowing = false);

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
    Task<Filter> GetFilter(string filterId);

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
    Task<Filter> UpdateFilter(string filterId, string? phrase = null, FilterContext? context = null, bool? irreversible = null, bool? wholeWord = null, uint? expiresIn = null);

    /// <summary>
    /// Deleting a text filter
    /// </summary>
    /// <param name="filterId"></param>
    Task DeleteFilter(string filterId);

    /// <summary>
    /// Get a poll
    /// </summary>
    /// <param name="id">The ID of the poll</param>
    /// <returns>Returns Poll</returns>
    Task<Poll> GetPoll(string id);

    /// <summary>
    /// Vote on a poll.
    /// </summary>
    /// <param name="id">The ID of the poll</param>
    /// <param name="choices">Array of choice indices</param>
    /// <returns>Returns Poll</returns>
    Task<Poll> Vote(string id, IEnumerable<int> choices);

    #endregion

    #region MastodonClient.Account

    /// <summary>
    /// View information about a profile.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns an Account</returns>
    Task<Account> GetAccount(string accountId);

    /// <summary>
    /// Get the user's own Account with Source
    /// </summary>
    /// <returns>Returns the user's own Account with Source</returns>
    Task<Account> GetCurrentUser();



    /// <summary>
    /// Update the user's display and preferences.
    /// </summary>
    /// <param name="discoverable">Whether the account should be shown in the profile directory</param>
    /// <param name="bot">Whether the account has a bot flag</param>
    /// <param name="display_name">The display name to use for the profile</param>
    /// <param name="note">The account bio</param>
    /// <param name="avatar">Avatar image</param>
    /// <param name="header">Header image</param>
    /// <param name="locked">Whether manual approval of follow requests is required</param>
    /// <param name="source_privacy">Default post privacy for authored statuses</param>
    /// <param name="source_sensitive">Whether to mark authored statuses as sensitive by default</param>
    /// <param name="source_language">Default language to use for authored statuses (ISO6391)</param>
    /// <param name="fields_attributes">Profile metadata name and value. (By default, max 4 fields and 255 characters per property/value)</param>
    /// <returns>Returns the user's own Account with Source</returns>
    Task<Account> UpdateCredentials(bool? discoverable = null,
                                    bool? bot = null,
                                    string? display_name = null,
                                    string? note = null,
                                    MediaDefinition? avatar = null,
                                    MediaDefinition? header = null,
                                    bool? locked = null,
                                    Visibility? source_privacy = null,
                                    bool? source_sensitive = null,
                                    string? source_language = null,
                                    IEnumerable<Field>? fields_attributes = null);

    /// <summary>
    /// Getting an account's relationships
    /// </summary>
    /// <param name="id">Account ID</param>
    /// <returns>Returns an array of Relationships of the current user to a given account</returns>
    Task<IEnumerable<Relationship>> GetAccountRelationships(string id);

    /// <summary>
    /// Getting an account's relationships
    /// </summary>
    /// <param name="id">Account IDs</param>
    /// <returns>Returns an array of Relationships of the current user to a list of given accounts</returns>
    Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<string> ids);

    /// <summary>
    /// Getting an account's followers
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    Task<MastodonList<Account>> GetAccountFollowers(string accountId, ArrayOptions? options = null);

    /// <summary>
    /// Getting who account is following
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    Task<MastodonList<Account>> GetAccountFollowing(string accountId, ArrayOptions? options = null);

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
    Task<MastodonList<Status>> GetAccountStatuses(string accountId, ArrayOptions? options = null, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false);

    /// <summary>
    /// Fetching a list of follow requests
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
    Task<MastodonList<Account>> GetFollowRequests(ArrayOptions? options = null);

    /// <summary>
    /// Authorizing follow requests
    /// </summary>
    /// <param name="accountId">The id of the account to authorize</param>
    Task AuthorizeRequest(string accountId);

    /// <summary>
    /// Rejecting follow requests
    /// </summary>
    /// <param name="accountId">The id of the account to reject</param>
    Task RejectRequest(string accountId);

    /// <summary>
    /// Listing accounts the user had past positive interactions with, but is not following yet
    /// </summary>
    /// <returns>Returns array of Account</returns>
    Task<IEnumerable<Account>> GetFollowSuggestions();

    /// <summary>
    /// Removing account from suggestions
    /// </summary>
    /// <param name="accountId">The account ID to remove</param>
    Task DeleteFollowSuggestion(string accountId);

    /// <summary>
    /// Fetching a user's favourites
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
    Task<MastodonList<Status>> GetFavourites(ArrayOptions? options = null);

    /// <summary>
    /// View your bookmarks.
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Statuses the user has bookmarked</returns>
    Task<MastodonList<Status>> GetBookmarks(ArrayOptions? options = null);

    /// <summary>
    /// View your featured tags
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<FeaturedTag>> GetFeaturedTags();

    /// <summary>
    /// Feature a tag
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<FeaturedTag> FeatureTag(string name);

    /// <summary>
    /// Unfeature a tag
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task UnfeatureTag(string id);

    /// <summary>
    /// Shows your 10 most-used tags, with usage history for the past week.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Tag>> GetFeaturedTagsSuggestions();

    #endregion

    #region MastodonClient.AccountActions

    /// <summary>
    /// Following an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="reblogs">Whether the followed account’s reblogs will show up in the home timeline</param>
    /// <returns>Returns the target Account</returns>
    Task<Relationship> Follow(string accountId, bool reblogs = true);

    /// <summary>
    /// Unfollowing an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    Task<Relationship> Unfollow(string accountId);

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
    Task<Relationship> Block(string accountId);

    /// <summary>
    /// Unblocking an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    Task<Relationship> Unblock(string accountId);

    /// <summary>
    /// Fetching a user's blocks
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
    Task<MastodonList<Account>> GetBlocks(ArrayOptions? options = null);

    /// <summary>
    /// Muting an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="notifications">Whether the mute will mute notifications or not</param>
    /// <returns>Returns the target Account</returns>
    Task<Relationship> Mute(string accountId, bool notifications = true);

    /// <summary>
    /// Unmuting an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the target Account</returns>
    Task<Relationship> Unmute(string accountId);

    /// <summary>
    /// Fetching a user's mutes
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
    Task<MastodonList<Account>> GetMutes(ArrayOptions? options = null);

    /// <summary>
    /// Fetching a user's blocked domains
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of strings</returns>
    Task<MastodonList<string>> GetDomainBlocks(ArrayOptions? options = null);

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
    Task<Relationship> Endorse(string accountId);

    /// <summary>
    /// Undoing endorse of an account
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns the updated Relationships with the target Account</returns>
    Task<Relationship> Unendorse(string accountId);

    /// <summary>
    /// Get saved timeline position
    /// </summary>
    /// <param name="home"></param>
    /// <param name="notifications"></param>
    /// <returns></returns>
    Task<Marker> GetMarkers(bool home = false, bool notifications = false);

    /// <summary>
    /// Save position in timeline
    /// </summary>
    /// <param name="homeLastReadId"></param>
    /// <param name="notificationLastReadId"></param>
    /// <returns></returns>
    Task<Marker> SetMarkers(string? homeLastReadId = null, string? notificationLastReadId = null);

    /// <summary>
    /// View information about a single tag
    /// </summary>
    /// <param name="tag">The name of the hashtag</param>
    /// <returns></returns>
    Task<Tag> GetTagInfo(string tag);

    /// <summary>
    /// Follow a hashtag
    /// </summary>
    /// <param name="tag">The name of the hashtag</param>
    /// <returns></returns>
    Task<Tag> FollowTag(string tag);

    /// <summary>
    /// Unfollow a hashtag
    /// </summary>
    /// <param name="tag">The name of the hashtag</param>
    /// <returns></returns>
    Task<Tag> UnfollowTag(string tag);

    /// <summary>
    /// View all followed tags
    /// </summary>
    /// <returns></returns>
    Task<MastodonList<Tag>> ViewFollowedTags(ArrayOptions? options = null);

    #endregion

    #region MastodonClient.Status

    /// <summary>
    /// Fetching a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns a Status</returns>
    Task<Status> GetStatus(string statusId);

    /// <summary>
    /// Getting status context
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns a Context</returns>
    Task<Context> GetStatusContext(string statusId);

    /// <summary>
    /// Getting a card associated with a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns a Card</returns>
    Task<Card> GetStatusCard(string statusId);

    /// <summary>
    /// Getting who reblogged a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    Task<MastodonList<Account>> GetRebloggedBy(string statusId, ArrayOptions? options = null);

    /// <summary>
    /// Getting who favourited a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Accounts</returns>
    Task<MastodonList<Account>> GetFavouritedBy(string statusId, ArrayOptions? options = null);

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
    /// <param name="poll">Nested parameters to attach a poll to the status</param>
    /// <returns>Returns Status</returns>
    Task<Status> PublishStatus(string status, Visibility? visibility = null, string? replyStatusId = null, IEnumerable<string>? mediaIds = null, bool sensitive = false, string? spoilerText = null, DateTime? scheduledAt = null, string? language = null, PollParameters? poll = null);

    /// <summary>
    /// Edit a given status to change its text, sensitivity, media attachments, or poll. Note that editing a poll’s options will reset the votes.
    /// </summary>
    /// <param name="statusId">The ID of the status in the database.</param>
    /// <param name="status">The plain text content of the status.</param>
    /// <param name="mediaIds">Include Attachment IDs to be attached as media. If provided, status becomes optional, and poll cannot be used.</param>
    /// <param name="sensitive">Whether the status should be marked as sensitive.</param>
    /// <param name="spoilerText">The plain text subject or content warning of the status.</param>
    /// <param name="language">ISO 639 language code for the status.</param>
    /// <param name="poll">Nested parameters to attach a poll to the status</param>
    /// <returns></returns>
    Task<Status> EditStatus(string statusId, string status, IEnumerable<string>? mediaIds = null, bool sensitive = false, string? spoilerText = null, string? language = null, PollParameters? poll = null);

    /// <summary>
    /// Deleting a status
    /// </summary>
    /// <param name="statusId"></param>
    Task DeleteStatus(string statusId);

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
    Task<ScheduledStatus> GetScheduledStatus(string scheduledStatusId);

    /// <summary>
    /// Update Scheduled status. Only scheduled_at can be changed. To change the content, delete it and post a new status.
    /// </summary>
    /// <param name="scheduledStatusId"></param>
    /// <param name="scheduledAt">DateTime to schedule posting of status</param>
    /// <returns>Returns ScheduledStatus</returns>
    Task<ScheduledStatus> UpdateScheduledStatus(string scheduledStatusId, DateTime? scheduledAt);

    /// <summary>
    /// Remove Scheduled status.
    /// </summary>
    /// <param name="scheduledStatusId"></param>
    Task DeleteScheduledStatus(string scheduledStatusId);

    /// <summary>
    /// Reblogging a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Reblog(string statusId);

    /// <summary>
    /// Unreblogging a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Unreblog(string statusId);

    /// <summary>
    /// Favouriting a status
    /// </summary>
    /// <param name="statusId">The ID of the Status in the database</param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Favourite(string statusId);

    /// <summary>
    /// Unfavouriting a status
    /// </summary>
    /// <param name="statusId">The ID of the Status in the database</param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Unfavourite(string statusId);

    /// <summary>
    /// Privately bookmark a status
    /// </summary>
    /// <param name="statusId">The ID of the Status in the database</param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Bookmark(string statusId);

    /// <summary>
    /// Remove a status from your private bookmarks
    /// </summary>
    /// <param name="statusId">The ID of the Status in the database</param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Unbookmark(string statusId);

    /// <summary>
    /// Muting a conversation of a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    Task<Status> MuteConversation(string statusId);

    /// <summary>
    /// Unmuting a conversation of a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    Task<Status> UnmuteConversation(string statusId);

    /// <summary>
    /// Pinning a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Pin(string statusId);

    /// <summary>
    /// Unpinning a status
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns>Returns the target Status</returns>
    Task<Status> Unpin(string statusId);

    #endregion

    #region MastodonClient.Timeline

    /// <summary>
    /// Retrieving Home timeline
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Statuses, most recent ones first</returns>
    Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions? options = null);

    /// <summary>
    /// Conversations (direct messages) for an account
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns array of Conversation</returns>
    Task<MastodonList<Conversation>> GetConversations(ArrayOptions? options = null);

    /// <summary>
    /// Remove conversation
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteConversation(string id);

    /// <summary>
    /// Mark as read
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Conversation> MarkAsRead(string id);

    /// <summary>
    /// Retrieving Public timeline
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <param name="local">Only return statuses originating from this instance</param>
    /// <returns>Returns an array of Statuses, most recent ones first</returns>
    Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions? options = null, bool local = false, bool onlyMedia = false);

    /// <summary>
    /// Retrieving Tag timeline
    /// </summary>
    /// <param name="hashtag">The tag to retieve</param>
    /// <param name="local">Only return statuses originating from this instance</param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Statuses, most recent ones first</returns>
    Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions? options = null, bool local = false, bool onlyMedia = false);

    /// <summary>
    /// Retrieving List timeline
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns an array of Statuses, most recent ones first</returns>
    Task<MastodonList<Status>> GetListTimeline(long listId, ArrayOptions? options = null);

    TimelineStreaming GetPublicStreaming();

    TimelineStreaming GetPublicLocalStreaming();

    TimelineStreaming GetUserStreaming();

    TimelineStreaming GetHashtagStreaming(string hashtag);

    TimelineStreaming GetHashtagLocalStreaming(string hashtag);

    TimelineStreaming GetDirectMessagesStreaming();

    #endregion
}
