# Masto.NET API 

## How to use

To use Mastonet, you need a `MastodonClient` object (see [README.md](https://github.com/glacasa/Mastonet/blob/master/README.md) ), and you have C# methods to call every [Mastodon API](https://github.com/tootsuite/documentation/blob/master/Using-the-API/API.md) method.

If a method is missing, please [submit an issue](https://github.com/glacasa/Mastonet/issues)

## Working with lists

Several Mastodon methods returns arrays of different types of items. Default usage will usually return the last 20 or 40 items, and you can add params to you request to handle pagination.

Those methods contains 3 parameters : `maxId`, `sinceId`, and `limit` ; and return a `MastodonList<T>`.  
`MastodonList<T>` inherits `List<T>` and can be used the same way, and contains 2 more properties : `NextPageMaxId` and `PreviousPageSinceId`.

The default behaviour of a client app should be like this :

- On the first load, the methods is called with no params, and gets the last items
- If you want to load older items (next page), call the method with `maxId` param, defined with the `NextPageMaxId` property from the previous list
- To get newer items (previous page), call the method with `sinceId` param, defined with the `PreviousPageSinceId` property from the previous list

You can always let the `limit` to its default value or define the number of items you want to load.

All the methods can be called with the 3 params `maxId`, `sinceId`, and `limit`, or using an `ArrayOptions` object. The result will be the same. Only the methods with `ArrayOptions` are presented below.

## Methods

### Instance
```cs
public Task<Instance> GetInstance();
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

public Task<Report> Report(long accountId, IEnumerable<long> statusIds, string comment);
```
### Search
 ```cs
public Task<Results> Search(string q, bool resolve = false);

public Task<IEnumerable<Account>> SearchAccounts(string q, int? limit = null);
```
### Account
```cs
public Task<Account> GetAccount(long accountId);

public Task<Account> GetCurrentUser();

public Task<Account> UpdateCredentials(string display_name = null, string note = null, MediaDefinition avatar = null, MediaDefinition header = null);

public Task<IEnumerable<Relationship>> GetAccountRelationships(long id);
public Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<long> ids);

public Task<MastodonList<Account>> GetAccountFollowers(long accountId, ArrayOptions options);

public Task<MastodonList<Account>> GetAccountFollowing(long accountId, ArrayOptions options);

public Task<MastodonList<Status>> GetAccountStatuses(long accountId, ArrayOptions options, bool onlyMedia = false, bool excludeReplies = false);

public Task<MastodonList<Account>> GetFollowRequests(ArrayOptions options);

public Task AuthorizeRequest(long accountId);

public Task RejectRequest(long accountId);
	
public Task<MastodonList<Status>> GetFavourites(ArrayOptions options);
```
### Account actions
```cs
public Task<Relationship> Follow(long accountId);

public Task<Relationship> Unfollow(long accountId);

public Task<Account> Follow(string uri);

public Task<Relationship> Block(long accountId);

public Task<Relationship> Unblock(long accountId);

public Task<MastodonList<Account>> GetBlocks(ArrayOptions options);

public Task<Relationship> Mute(long accountId);

public Task<Relationship> Unmute(long accountId);

public Task<MastodonList<Account>> GetMutes(ArrayOptions options);

public Task<MastodonList<string>> GetDomainBlocks(ArrayOptions options);

public Task BlockDomain(string domain);

public Task UnblockDomain(string domain);
```
### Statuses
```cs
public Task<Status> GetStatus(long statusId);

public Task<Context> GetStatusContext(long statusId);

public Task<Card> GetStatusCard(long statusId);

public Task<MastodonList<Account>> GetRebloggedBy(long statusId, ArrayOptions options);

public Task<MastodonList<Account>> GetFavouritedBy(long statusId, ArrayOptions options);

public Task<Status> PostStatus(string status, Visibility visibility, long? replyStatusId = null, IEnumerable<long> mediaIds = null, bool sensitive = false, string spoilerText = null);

public Task DeleteStatus(long statusId);

public Task<Status> Reblog(long statusId);

public Task<Status> Unreblog(long statusId);

public Task<Status> Favourite(long statusId);

public Task<Status> Unfavourite(long statusId);

public Task<Status> MuteConversation(long statusId);

public Task<Status> UnmuteConversation(long statusId);
```
### Timelines
```cs
public Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions options);

public Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions options, bool local = false);

public Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false);
```
