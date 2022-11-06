# Masto.NET API 

## How to use

To use Mastonet, you need a `MastodonClient` object (see [README.md](https://github.com/glacasa/Mastonet/blob/master/README.md) ), and you have C# methods to call every [Mastodon API](https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md) method.

If a method is missing, please [submit an issue](https://github.com/glacasa/Mastonet/issues).

## Working with pagination

Some Mastodon REST APIs that return an array of items, also return pagination hint in their response header.

Methods corresponding to these APIs return `MastodonList<T>` and have 2 overloads: One takes 3 parameters: `maxId`, `sinceId` and `limit`, and the other takes a single `ArrayOptions options` parameter.
`ArrayOptions` is basically a bundle of the 3 parameters (it has `MaxId`, `SinceId` and `Limit` properties), except it can have `MinId` property supported since Mastodon v2.6.0.

The returned `MastodonList<T>` inherits `List<T>` and can be used the same way, and contains 3 more properties : `NextPageMaxId`, `PreviousPageSinceId` and `PreviousPageMinId`.

The default behaviour of a client app should be like this :

- On the first load, the methods is called with no params, and gets the last items
- If you want to load older items (next page), call the method with `maxId` param, defined with the `NextPageMaxId` property from the previous list
- To get newer items (previous page), call the method with `minId` param, defined with the `PreviousPageMinId` property from the previous list
- If you want to load newest items but don't need already fetched ones, call the method with `options` param with `MinId` property set, defined with the `PreviousPageSinceId` property from the previous list

You can always let the `limit` to its default value or define the number of items you want to load.

Note that the 3-parameter overloads don't have `minId` for backward compatibility. Use the overloads with `ArrayOptions` if you need it.

## Methods

### Instance
```cs
public Task<Instance> GetInstance();
```
### Lists
```cs
public Task<IEnumerable<List>> GetLists();

public Task<IEnumerable<List>> GetListsContainingAccount(long accountId);

public Task<MastodonList<Account>> GetListAccounts(long listId, ArrayOptions options);

public Task<List> GetList(long listId);

public Task<List> CreateList(string title);

public Task<List> UpdateList(long listId, string newTitle);

public Task DeleteList(long listId);

public Task AddAccountsToList(long listId, IEnumerable<long> accountIds);
public Task AddAccountsToList(long listId, IEnumerable<Account> accounts);

public Task RemoveAccountsFromList(long listId, IEnumerable<long> accountIds);
public Task RemoveAccountsFromList(long listId, IEnumerable<Account> accounts);
```
### Media
```cs
public Task<Attachment> UploadMedia(MediaDefinition media);
```
### Notifications
```cs
public Task<MastodonList<Notification>> GetNotifications(ArrayOptions options);

public Task<Notification> GetNotification(long notificationId);

public Task ClearNotifications();
```
### Reports
```cs
public Task<MastodonList<Report>> GetReports(ArrayOptions options);

public Task<Report> Report(long accountId, IEnumerable<long> statusIds = null, string comment = null, bool? forward = null);
```
### Search
```cs
public Task<Results> Search(string q, bool resolve = false);

public Task<ResultsV2> SearchV2(string q, bool resolve = false);

public Task<IEnumerable<Account>> SearchAccounts(string q, int? limit = null, bool resolve = false, bool following = false);
```
### Filters
```cs
public Task<IEnumerable<Filter>> GetFilters();

public Task<Filter> CreateFilter(string phrase, FilterContext context, bool irreversible = false, bool wholeWord = false, uint? expiresIn = null);

public Task<Filter> GetFilter(long filterId);

public Task<Filter> UpdateFilter(long filterId, string phrase = null, FilterContext? context = null, bool? irreversible = null, bool? wholeWord = null, uint? expiresIn = null);

public Task DeleteFilter(long filterId);
```
### Polls
```cs
public Task<Poll> GetPoll(long id);

public Task<Poll> Vote(long id, IEnumerable<int> choices);
```
### Account
```cs
public Task<Account> GetAccount(long accountId);

public Task<Account> GetCurrentUser();

public Task<Account> UpdateCredentials(string display_name = null, string note = null, MediaDefinition avatar = null, MediaDefinition header = null, bool? locked = null, Visibility? source_privacy = null, bool? source_sensitive = null, string source_language = null, IEnumerable<AccountField> fields_attributes = null);

public Task<IEnumerable<Relationship>> GetAccountRelationships(long id);
public Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<long> ids);

public Task<MastodonList<Account>> GetAccountFollowers(long accountId, ArrayOptions options);

public Task<MastodonList<Account>> GetAccountFollowing(long accountId, ArrayOptions options);

public Task<MastodonList<Status>> GetAccountStatuses(long accountId, ArrayOptions options, bool onlyMedia = false, bool excludeReplies = false, bool pinned = false, bool excludeReblogs = false);

public Task<MastodonList<Account>> GetFollowRequests(ArrayOptions options);

public Task AuthorizeRequest(long accountId);

public Task RejectRequest(long accountId);

public Task<IEnumerable<Account>> GetFollowSuggestions();

public Task DeleteFollowSuggestion(long accountId);
	
public Task<MastodonList<Status>> GetFavourites(ArrayOptions options);
```
### Account actions
```cs
public Task<Relationship> Follow(long accountId, bool reblogs = true);

public Task<Relationship> Unfollow(long accountId);

public Task<Account> Follow(string uri);

public Task<Relationship> Block(long accountId);

public Task<Relationship> Unblock(long accountId);

public Task<MastodonList<Account>> GetBlocks(ArrayOptions options);

public Task<Relationship> Mute(long accountId, bool notifications = true);

public Task<Relationship> Unmute(long accountId);

public Task<MastodonList<Account>> GetMutes(ArrayOptions options);

public Task<MastodonList<string>> GetDomainBlocks(ArrayOptions options);

public Task BlockDomain(string domain);

public Task UnblockDomain(string domain);

public Task<MastodonList<Account>> GetEndorsements();

public Task<Relationship> Endorse(long accountId);

public Task<Relationship> Unendorse(long accountId);
```
### Statuses
```cs
public Task<Status> GetStatus(long statusId);

public Task<Context> GetStatusContext(long statusId);

public Task<Card> GetStatusCard(long statusId);

public Task<MastodonList<Account>> GetRebloggedBy(long statusId, ArrayOptions options);

public Task<MastodonList<Account>> GetFavouritedBy(long statusId, ArrayOptions options);

public Task<Status> PostStatus(string status, Visibility? visibility = null, long? replyStatusId = null, IEnumerable<long> mediaIds = null, bool sensitive = false, string spoilerText = null, DateTime? scheduledAt = null, string language = null, PollParameters poll = null);

public Task DeleteStatus(long statusId);

public Task<IEnumerable<ScheduledStatus>> GetScheduledStatuses();

public Task<ScheduledStatus> GetScheduledStatus(long scheduledStatusId);

public Task<ScheduledStatus> UpdateScheduledStatus(long scheduledStatusId, DateTime? scheduledAt);

public Task DeleteScheduledStatus(long scheduledStatusId);

public Task<Status> Reblog(long statusId);

public Task<Status> Unreblog(long statusId);

public Task<Status> Favourite(long statusId);

public Task<Status> Unfavourite(long statusId);

public Task<Status> MuteConversation(long statusId);

public Task<Status> UnmuteConversation(long statusId);

public Task<Status> Pin(long statusId);

public Task<Status> Unpin(long statusId);
```
### Timelines
```cs
public Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions options);

public Task<MastodonList<Conversation>> GetConversations(ArrayOptions options);

public Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions options, bool local = false, bool onlyMedia = false);

public Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false, bool onlyMedia = false);

public Task<MastodonList<Status>> GetListTimeline(long listId, ArrayOptions options);
```
