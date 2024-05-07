using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mastonet;

public partial class MastodonClient : BaseHttpClient, IMastodonClient
{
    #region Ctor

    public MastodonClient(string instance, string accessToken)
        : this(instance, accessToken, DefaultHttpClient.Instance)
    {
    }

    public MastodonClient(string instance, string accessToken, HttpClient client)
        : base(client)
    {
        this.Instance = instance;
        this.AccessToken = accessToken;

        this.instanceGetter = new Lazy<Task<InstanceV2>>(this.GetInstanceV2);
    }

    #endregion

    #region Instances

    /// <summary>
    /// Getting instance information
    /// </summary>
    /// <returns>Returns the current Instance. Does not require authentication</returns>
    [Obsolete("This method is deprecated on Mastodon v4. Use GetInstanceV2() instead.")]
    public Task<Instance> GetInstance() 
    {
        return this.Get<Instance>("/api/v1/instance");
    }
    
    /// <summary>
    /// Getting instance information
    /// </summary>
    /// <returns>Returns the current Instance. Does not require authentication</returns>
    public Task<InstanceV2> GetInstanceV2()
    {
        return this.Get<InstanceV2>("/api/v2/instance");
    }

    /// <summary>
    /// List of connected domains
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<string>> GetInstancePeers()
    {
        return Get<IEnumerable<string>>("/api/v1/instance/peers");
    }

    /// <summary>
    /// Weekly activity
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Activity>> GetInstanceActivity()
    {
        return Get<IEnumerable<Activity>>("/api/v1/instance/activity");
    }

    /// <summary>
    /// Tags that are being used more frequently within the past week.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Tag>> GetTrendingTags()
    {
        return Get<IEnumerable<Tag>>("/api/v1/trends/tags");
    }

    /// <summary>
    /// Statuses that have been interacted with more than others.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public Task<MastodonList<Status>> GetTrendingStatuses(int? offset = null, int? limit = null)
    {
        var queryParams = "";

        if(offset.HasValue)
        {
            queryParams = "?offset=" + offset.Value;
        }
        if(limit.HasValue)
        {
            queryParams += (queryParams != "" ? "&" : "?") + "limit=" + limit.Value;
        }

        return GetMastodonList<Status>("/api/v1/trends/statuses" + queryParams);
    }

    /// <summary>
    /// A directory of profiles that your website is aware of.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <param name="order"></param>
    /// <param name="local"></param>
    /// <returns></returns>
    public Task<IEnumerable<Account>> GetDirectory(int? offset, int? limit, DirectoryOrder? order, bool? local)
    {
        var queryParams = "";

        if (offset.HasValue)
        {
            queryParams = "?offset=" + offset.Value;
        }

        if (limit.HasValue)
        {
            queryParams += (queryParams != "" ? "&" : "?") + "limit=" + limit.Value;
        }

        if (order.HasValue)
        {
            queryParams += (queryParams != "" ? "&" : "?") + "order=" + order.Value.ToString().ToLowerInvariant();
        }

        if (local.HasValue)
        {
            queryParams += (queryParams != "" ? "&" : "?") + "local=" + local.Value;
        }

        return Get<IEnumerable<Account>>("/api/v1/directory" + queryParams);
    }


    /// Get all announcements set by administration
    /// </summary>
    /// <param name="withDismissed">If true, response will include announcements dismissed by the user</param>
    /// <returns></returns>
    public Task<IEnumerable<Announcement>> GetAnnouncements(bool withDismissed = false)
    {
        return Get<IEnumerable<Announcement>>("/api/v1/announcements");
    }

    /// <summary>
    /// Allows a user to mark the announcement as read
    /// </summary>
    /// <param name="id"></param>
    public Task DismissAnnouncement(string id)
    {
        return Post($"/api/v1/announcements/{id}/dismiss");
    }

    /// <summary>
    /// React to an announcement with an emoji
    /// </summary>
    /// <param name="id">Local ID of an announcement in the database</param>
    /// <param name="emoji">Unicode emoji, or shortcode of custom emoji</param>
    public Task AddReactionToAnnouncement(string id, string emoji)
    {
        return Put($"/api/v1/announcements/{id}/reactions/{emoji}");
    }

    /// <summary>
    /// Undo a react emoji to an announcement
    /// </summary>
    /// <param name="id">Local ID of an announcement in the database</param>
    /// <param name="emoji">Unicode emoji, or shortcode of custom emoji</param>
    /// <returns></returns>
    public Task RemoveReactionFromAnnouncement(string id, string emoji)
    {
        return Delete($"/api/v1/announcements/{id}/reactions/{emoji}");
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

    /// <summary>
    /// User’s lists that a given account is part of.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns>Returns array of List</returns>
    public Task<IEnumerable<List>> GetListsContainingAccount(string accountId)
    {
        return this.Get<IEnumerable<List>>($"/api/v1/accounts/{accountId}/lists");
    }

    /// <summary>
    /// Accounts that are in a given list.
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns array of Account</returns>
    public Task<MastodonList<Account>> GetListAccounts(string listId, ArrayOptions? options = null)
    {
        var url = $"/api/v1/lists/{listId}/accounts";
        if (options != null)
        {
            url += "?" + options.ToQueryString();
        }

        return GetMastodonList<Account>(url);
    }

    /// <summary>
    /// Get a list.
    /// </summary>
    /// <param name="listId"></param>
    /// <returns>Returns List</returns>
    public Task<List> GetList(string listId)
    {
        return this.Get<List>("/api/v1/lists/" + listId);
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

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("title", title),
        };

        return this.Post<List>("/api/v1/lists", data);
    }

    /// <summary>
    /// Update a list.
    /// </summary>
    /// <param name="title">The title of the list</param>
    /// <returns>The list updated</returns>
    public Task<List> UpdateList(string listId, string newTitle)
    {
        if (string.IsNullOrEmpty(newTitle))
        {
            throw new ArgumentException("The title is required", nameof(newTitle));
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("title", newTitle),
        };

        return this.Put<List>("/api/v1/lists/" + listId, data);
    }

    /// <summary>
    /// Remove a list.
    /// </summary>
    /// <param name="listId"></param>
    public Task DeleteList(string listId)
    {
        return this.Delete("/api/v1/lists/" + listId);
    }

    /// <summary>
    /// Add accounts to a list.
    /// Only accounts already followed by the user can be added to a list.
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="accountIds">Array of account IDs</param>
    public Task AddAccountsToList(string listId, IEnumerable<string> accountIds)
    {
        if (accountIds == null || !accountIds.Any())
        {
            throw new ArgumentException("Accounts are required", nameof(accountIds));
        }

        var data = accountIds.Select(id => new KeyValuePair<string, string>("account_ids[]", id));

        return this.Post($"/api/v1/lists/{listId}/accounts", data);
    }

    /// <summary>
    /// Add accounts to a list.
    /// Only accounts already followed by the user can be added to a list.
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="accounts">Array of Accounts</param>
    public Task AddAccountsToList(string listId, IEnumerable<Account> accounts)
    {
        return AddAccountsToList(listId, accounts.Select(account => account.Id));
    }

    /// <summary>
    /// Remove accounts from a list.
    /// </summary>
    /// <param name="listId">List Id</param>
    /// <param name="accountIds">Array of Account IDs</param>
    public Task RemoveAccountsFromList(string listId, IEnumerable<string> accountIds)
    {
        if (accountIds == null || !accountIds.Any())
        {
            throw new ArgumentException("Accounts are required", nameof(accountIds));
        }

        var data = accountIds.Select(id => new KeyValuePair<string, string>("account_ids[]", id));

        return this.Delete($"/api/v1/lists/{listId}/accounts", data);
    }

    /// <summary>
    /// Remove accounts from a list.
    /// </summary>
    /// <param name="listId">List Id</param>
    /// <param name="accountIds">Array of Accounts</param>
    public Task RemoveAccountsFromList(string listId, IEnumerable<Account> accounts)
    {
        return RemoveAccountsFromList(listId, accounts.Select(account => account.Id));
    }

    #endregion

    #region Media
    /// <summary>
    /// Get a media attachment, before it is attached to a status and posted, but after it is accepted for processing. Use this method to check that the full-sized media has finished processing.
    /// </summary>
    /// <param name="attachmentId">The ID of the MediaAttachment in the database</param>
    /// <returns></returns>
    public Task<Attachment> GetMediaAttachment(string attachmentId)
    {
        return Get<Attachment>($"/api/v1/media/{attachmentId}");
    }
    /// <summary>
    /// Uploading a media attachment
    /// </summary>
    /// <param name="data">Media stream to be uploaded</param>
    /// <param name="fileName">Media file name (must contains extension ex: .png, .jpg, ...)</param>
    /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
    /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
    /// <returns>Returns an Attachment that can be used when creating a status</returns>
    public Task<Attachment> UploadMedia(Stream data, string fileName = "file", string? description = null,
        AttachmentFocusData? focus = null)
    {
        return UploadMedia(new MediaDefinition(data, fileName), description, focus);
    }

    /// <summary>
    /// Uploading a media attachment
    /// </summary>
    /// <param name="media">Media to be uploaded</param>
    /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
    /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
    /// <returns>Returns an Attachment that can be used when creating a status</returns>
    public Task<Attachment> UploadMedia(MediaDefinition media, string? description = null,
        AttachmentFocusData? focus = null)
    {
        media.ParamName = "file";
        var list = new List<MediaDefinition>() { media };
        var data = new Dictionary<string, string>();
        if (description != null)
        {
            data.Add("description", description);
        }

        if (focus != null)
        {
            data.Add("focus", $"{focus.X},{focus.Y}");
        }

        return this.Post<Attachment>("/api/v2/media", data, list);
    }

    /// <summary>
    /// Update a media attachment. Can only be done before the media is attached to a status.
    /// </summary>
    /// <param name="mediaId">Media ID</param>
    /// <param name="description">A plain-text description of the media for accessibility (max 420 chars)</param>
    /// <param name="focus">Two floating points. See <see cref="https://docs.joinmastodon.org/api/rest/media/#focal-points">focal points</see></param>
    /// <returns>Returns an Attachment that can be used when creating a status</returns>
    public Task<Attachment> UpdateMedia(string mediaId, string? description = null, AttachmentFocusData? focus = null)
    {
        var data = new Dictionary<string, string>();
        if (description != null)
        {
            data.Add("description", description);
        }

        if (focus != null)
        {
            data.Add("focus", $"{focus.X},{focus.Y}");
        }

        return Put<Attachment>("/api/v1/media/" + mediaId, data);
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
    /// <param name="options">Define the first and last items to get</param>
    /// <param name="excludeTypes">Types to exclude</param>
    /// <returns>Returns a list of Notifications for the authenticated user</returns>
    public Task<MastodonList<Notification>> GetNotifications(ArrayOptions? options = null,
        NotificationType excludeTypes = NotificationType.None)
    {
        var url = "/api/v1/notifications";
        var queryParams = "";
        if (options != null)
        {
            queryParams += "?" + options.ToQueryString();
        }

        if (excludeTypes.HasFlag(NotificationType.Follow))
        {
            queryParams += (queryParams != "" ? "&" : "?") + "exclude_types[]=follow";
        }

        if (excludeTypes.HasFlag(NotificationType.Favourite))
        {
            queryParams += (queryParams != "" ? "&" : "?") + "exclude_types[]=favourite";
        }

        if (excludeTypes.HasFlag(NotificationType.Reblog))
        {
            queryParams += (queryParams != "" ? "&" : "?") + "exclude_types[]=reblog";
        }

        if (excludeTypes.HasFlag(NotificationType.Mention))
        {
            queryParams += (queryParams != "" ? "&" : "?") + "exclude_types[]=mention";
        }

        if (excludeTypes.HasFlag(NotificationType.Poll))
        {
            queryParams += (queryParams != "" ? "&" : "?") + "exclude_types[]=poll";
        }

        if (excludeTypes.HasFlag(NotificationType.FollowRequest))
        {
            queryParams += (queryParams != "" ? "&" : "?") + "exclude_types[]=follow_request";
        }

        return GetMastodonList<Notification>(url + queryParams);
    }

    /// <summary>
    /// Getting a single notification
    /// </summary>
    /// <param name="notificationId"></param>
    /// <returns>Returns the Notification</returns>
    public Task<Notification> GetNotification(string notificationId)
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

    /// <summary>
    /// Delete a single notification from the server.
    /// </summary>
    /// <param name="notificationId"></param>
    /// <returns></returns>
    public Task DismissNotification(string notificationId)
    {
        return Post($"/api/v1/notifications/{notificationId}/dismiss");
    }

    #endregion

    #region Reports

    /// <summary>
    /// Fetching a user's reports
    /// </summary>
    /// <param name="options">Define the first and last items to get</param>
    /// <returns>Returns a list of Reports made by the authenticated user</returns>
    public Task<MastodonList<Report>> GetReports(ArrayOptions? options = null)
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
    /// <param name="forward">Whether to forward to the remote admin (in case of a remote account)</param>
    /// <returns>Returns the finished Report</returns>
    public Task<Report> Report(string accountId, IEnumerable<string>? statusIds = null, string? comment = null,
        bool? forward = null)
    {
        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("account_id", accountId),
        };
        if (statusIds != null)
        {
            foreach (var statusId in statusIds)
            {
                data.Add(new KeyValuePair<string, string>("status_ids[]", statusId));
            }
        }

        if (comment != null)
        {
            data.Add(new KeyValuePair<string, string>("comment", comment));
        }

        if (forward.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("forward", forward.Value.ToString().ToLowerInvariant()));
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
    /// <returns>Returns ResultsV2. If q is a URL, Mastodon will attempt to fetch the provided account or status. Otherwise, it will do a local account and hashtag search</returns>
    public Task<SearchResults> Search(string q, bool resolveNonLocalAccouns = false)
    {
        if (string.IsNullOrEmpty(q))
        {
            return Task.FromResult(new SearchResults());
        }

        string url = "/api/v2/search?q=" + Uri.EscapeDataString(q);
        if (resolveNonLocalAccouns)
        {
            url += "&resolve=true";
        }

        return Get<SearchResults>(url);
    }

    /// <summary>
    /// Searching for accounts
    /// </summary>
    /// <param name="q">What to search for</param>
    /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
    /// <param name="resolve">Attempt WebFinger look-up (default: false)</param>
    /// <param name="following">Only who the user is following (default: false)</param>
    /// <returns>Returns an array of matching Accounts. Will lookup an account remotely if the search term is in the username@domain format and not yet in the database.</returns>
    public Task<List<Account>> SearchAccounts(string q, int? limit = null, bool resolveNonLocalAccouns = false,
        bool onlyFollowing = false)
    {
        if (string.IsNullOrEmpty(q))
        {
            return Task.FromResult(new List<Account>());
        }

        string url = "/api/v1/accounts/search?q=" + Uri.EscapeDataString(q);
        if (limit.HasValue)
        {
            url += "&limit=" + limit.Value;
        }

        if (resolveNonLocalAccouns)
        {
            url += "&resolve=true";
        }

        if (onlyFollowing)
        {
            url += "&following=true";
        }

        return Get<List<Account>>(url);
    }

    #endregion

    #region Filters

    /// <summary>
    /// Listing all text filters the user has configured that potentially must be applied client-side
    /// </summary>
    /// <returns>Returns an array of filters</returns>
    public Task<IEnumerable<Filter>> GetFilters()
    {
        return Get<IEnumerable<Filter>>("/api/v1/filters");
    }

    /// <summary>
    /// Creating a new filter
    /// </summary>
    /// <param name="phrase">Keyword or phrase to filter</param>
    /// <param name="context">Filtering context. At least one context must be specified</param>
    /// <param name="irreversible">Irreversible filtering will only work in home and notifications contexts by fully dropping the records</param>
    /// <param name="wholeWord">Whether to consider word boundaries when matching</param>
    /// <param name="expiresIn">Number that indicates seconds. Filter will be expire in seconds after API processed. Leave null for no expiration</param>
    /// <returns>Returns a created filter</returns>
    public Task<Filter> CreateFilter(string phrase, FilterContext context, bool irreversible = false,
        bool wholeWord = false, uint? expiresIn = null)
    {
        if (string.IsNullOrEmpty(phrase))
        {
            throw new ArgumentException("The phrase is required", nameof(phrase));
        }

        if (context == 0)
        {
            throw new ArgumentException("At least one context must be specified", nameof(context));
        }

        var data = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("phrase", phrase) };
        foreach (FilterContext checkFlag in new[]
                     { FilterContext.Home, FilterContext.Notifications, FilterContext.Public, FilterContext.Thread })
        {
            if ((context & checkFlag) == checkFlag)
            {
                data.Add(new KeyValuePair<string, string>("context[]", checkFlag.ToString().ToLowerInvariant()));
            }
        }

        if (irreversible)
        {
            data.Add(new KeyValuePair<string, string>("irreversible", "true"));
        }

        if (wholeWord)
        {
            data.Add(new KeyValuePair<string, string>("whole_word", "true"));
        }

        if (expiresIn.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("expires_in", expiresIn.Value.ToString()));
        }

        return Post<Filter>("/api/v1/filters", data);
    }

    /// <summary>
    /// Getting a text filter
    /// </summary>
    /// <param name="filterId">Filter ID</param>
    /// <returns>Returns a filter</returns>
    public Task<Filter> GetFilter(string filterId)
    {
        return Get<Filter>($"/api/v1/filters/{filterId}");
    }

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
    public Task<Filter> UpdateFilter(string filterId, string? phrase = null, FilterContext? context = null,
        bool? irreversible = null, bool? wholeWord = null, uint? expiresIn = null)
    {
        if (context == 0)
        {
            throw new ArgumentException("At least one context to filter must be specified", nameof(context));
        }

        var data = new List<KeyValuePair<string, string>>();
        if (phrase != null)
        {
            data.Add(new KeyValuePair<string, string>("phrase", phrase));
        }

        if (context.HasValue)
        {
            foreach (FilterContext checkFlag in new[]
                     {
                         FilterContext.Home, FilterContext.Notifications, FilterContext.Public, FilterContext.Thread
                     })
            {
                if ((context & checkFlag) == checkFlag)
                {
                    data.Add(new KeyValuePair<string, string>("context[]", checkFlag.ToString().ToLowerInvariant()));
                }
            }
        }

        if (irreversible.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("irreversible",
                irreversible.Value.ToString().ToLowerInvariant()));
        }

        if (wholeWord.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("whole_word", wholeWord.Value.ToString().ToLowerInvariant()));
        }

        if (expiresIn.HasValue)
        {
            data.Add(new KeyValuePair<string, string>("expires_in", expiresIn.Value.ToString()));
        }

        return Put<Filter>($"/api/v1/filters/{filterId}", data);
    }

    /// <summary>
    /// Deleting a text filter
    /// </summary>
    /// <param name="filterId"></param>
    public Task DeleteFilter(string filterId)
    {
        return Delete($"/api/v1/filters/{filterId}");
    }

    #endregion

    #region Polls

    /// <summary>
    /// Get a poll
    /// </summary>
    /// <param name="id">The ID of the poll</param>
    /// <returns>Returns Poll</returns>
    public Task<Poll> GetPoll(string id)
    {
        return Get<Poll>("/api/v1/polls/" + id.ToString());
    }

    /// <summary>
    /// Vote on a poll.
    /// </summary>
    /// <param name="id">The ID of the poll</param>
    /// <param name="choices">Array of choice indices</param>
    /// <returns>Returns Poll</returns>
    public Task<Poll> Vote(string id, IEnumerable<int> choices)
    {
        var data = choices
            .Select(index => new KeyValuePair<string, string>("choices[]", index.ToString()))
            .ToArray();
        return Post<Poll>("/api/v1/polls/" + id.ToString() + "/votes", data);
    }

    #endregion

    #region Rate limits

    public event EventHandler<RateLimitEventArgs>? RateLimitsUpdated;

    private void UpdateRateLimits(HttpResponseMessage response)
    {
        if (RateLimitsUpdated != null)
        {
            // Get ratelimit info
            // https://docs.joinmastodon.org/api/rate-limits/

            var category = ApiCallCategory.Global;

            var requestMethod = response.RequestMessage?.Method;
            var requestPath = response.RequestMessage?.RequestUri?.AbsolutePath ?? "";
            if (requestMethod == HttpMethod.Post && requestPath == "/api/v1/media")
            {
                category = ApiCallCategory.MediaUpload;
            }
            if ((requestMethod == HttpMethod.Delete && requestPath.StartsWith("/api/v1/statuses/")) ||
                (requestMethod == HttpMethod.Post && requestPath.EndsWith("unreblog")))
            {
                category = ApiCallCategory.StatusDelete;
            }


            var headers = response.Headers;
            var limit = headers.GetValues("X-RateLimit-Limit").FirstOrDefault();
            var remaining = headers.GetValues("X-RateLimit-Remaining").FirstOrDefault();
            var reset = headers.GetValues("X-RateLimit-Reset").FirstOrDefault();

            if (!string.IsNullOrEmpty(limit) && int.TryParse(limit, out int intLimit) &&
                !string.IsNullOrEmpty(remaining) && int.TryParse(remaining, out int intRemaining) &&
                !string.IsNullOrEmpty(reset) && DateTime.TryParse(reset, out DateTime dateReset))
            {
                var rateLimitEventArgs = new RateLimitEventArgs
                {
                    RateLimitCategory = category,
                    Limit = intLimit,
                    Remaining = intRemaining,
                    Reset = dateReset
                };

                RateLimitsUpdated?.Invoke(this, rateLimitEventArgs);
            }
        }
    }

    #endregion

    protected override void OnResponseReceived(HttpResponseMessage response)
    {
        UpdateRateLimits(response);
    }
}
