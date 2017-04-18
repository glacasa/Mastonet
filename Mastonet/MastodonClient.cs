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
    public partial class MastodonClient
    {
        public string Instance { get; }

        public AppRegistration AppRegistration { get; set; }

        public string AccessToken { get; set; }


        #region Ctor

        public MastodonClient(AppRegistration appRegistration, string accessToken = null)
        {
            this.Instance = appRegistration.Instance;
            this.AppRegistration = appRegistration;
            this.AccessToken = accessToken;
        }

        #endregion

        #region Http helpers

        private void AddHttpHeader(HttpClient client)
        {
            if (AccessToken != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
            }
        }

        private async Task<string> Delete(string route)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);
            var response = await client.DeleteAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> Get(string route)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> Get<T>(string route)
            where T : class
        {
            var content = await Get(route);
            return TryDeserialize<T>(content);
        }

        private async Task<string> Post(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            AddHttpHeader(client);

            var content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> Post<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
            where T : class
        {
            var content = await Post(route, data);
            return TryDeserialize<T>(content);
        }

        private async Task<string> Patch(string route, IEnumerable<KeyValuePair<string, string>> data = null)
        {
            string url = "https://" + this.Instance + route;

            var client = new HttpClient();
            var method = new HttpMethod("PATCH");
            AddHttpHeader(client);

            var content = new FormUrlEncodedContent(data ?? Enumerable.Empty<KeyValuePair<string, string>>());
            var message = new HttpRequestMessage(method, url);
            message.Content = content;
            var response = await client.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<T> Patch<T>(string route, IEnumerable<KeyValuePair<string, string>> data = null)
            where T : class
        {
            var content = await Patch(route, data);
            return TryDeserialize<T>(content);
        }

        private T TryDeserialize<T>(string json)
        {
            //TODO handle error gracefully
            //var error = JsonConvert.DeserializeObject<Error>(json);
            //if (!string.IsNullOrEmpty(error.Description))
            //{
            //    throw new ServerErrorException(error);
            //}

            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region Apps

        /// <summary>
        /// Registering an application
        /// </summary>
        /// <param name="instance">Instance to connect</param>
        /// <param name="appName">Name of your application</param>
        /// <param name="scope">The rights needed by your application</param>
        /// <param name="website">URL to the homepage of your app</param>
        /// <returns></returns>
        public static async Task<AppRegistration> CreateApp(string instance, string appName, Scope scope, string website = null, string redirectUri = null)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("client_name", appName),
                new KeyValuePair<string, string>("scopes", GetScopeParam(scope)),
            };
            if (string.IsNullOrEmpty(redirectUri))
            {
                data.Add(new KeyValuePair<string, string>("redirect_uris", "urn:ietf:wg:oauth:2.0:oob"));
            }
            else
            {
                data.Add(new KeyValuePair<string, string>("redirect_uris", redirectUri));
            }
            if (!string.IsNullOrEmpty(website))
            {
                data.Add(new KeyValuePair<string, string>("website", website));
            }

            string url = $"https://{instance}/api/v1/apps";
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            var appRegistration = JsonConvert.DeserializeObject<AppRegistration>(responseJson);
            appRegistration.Instance = instance;
            appRegistration.Scope = scope;

            return appRegistration;
        }

        #endregion

        #region Auth

        public async Task<Auth> ConnectWithPassword(string email, string password)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("scope", GetScopeParam(AppRegistration.Scope)),
            };

            var auth = await Post<Auth>("/oauth/token", data);
            this.AccessToken = auth.AccessToken;
            return auth;
        }

        public async Task<Auth> ConnectWithCode(string code, string redirect_uri = null)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri ?? "urn:ietf:wg:oauth:2.0:oob"),
                new KeyValuePair<string, string>("code", code),
            };

            var auth = await Post<Auth>("/oauth/token", data);
            this.AccessToken = auth.AccessToken;
            return auth;
        }

        public string OAuthUrl(string redirectUri = null)
        {
            return $"https://{this.Instance}/oauth/authorize?response_type=code&client_id={this.AppRegistration.ClientId}&scope={GetScopeParam(AppRegistration.Scope).Replace(" ", "%20")}&redirect_uri={redirectUri ?? "urn:ietf:wg:oauth:2.0:oob"}";
        }

        private static string GetScopeParam(Scope scope)
        {
            var scopeParam = "";
            if ((scope & Scope.Read) == Scope.Read) scopeParam += " read";
            if ((scope & Scope.Write) == Scope.Write) scopeParam += " write";
            if ((scope & Scope.Follow) == Scope.Follow) scopeParam += " follow";

            return scopeParam.Trim();
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
        public Task<IEnumerable<Account>> GetBlocks(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Status>> GetFavourites(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Account>> GetFollowRequests(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Account>> GetAccountFollowers(int accountId, string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Account>> GetAccountFollowing(int accountId, string maxId = null, string sinceId = null)
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
            return this.Post<Account>($"/api/v1/follows");
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
        public Task<IEnumerable<Account>> GetMutes(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Notification>> GetNotifications(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Report>> GetReports(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Account>> SearchAccounts(string q, string maxId = null, string sinceId = null, int? limit = null)
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
        public Task<IEnumerable<Status>> GetAccountStatuses(int accountId, string maxId = null, string sinceId = null, bool onlyMedia = false, bool excludeReplies = false)
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
        public Task<IEnumerable<Account>> GetRebloggedBy(int statusId, string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Account>> GetFavouritedBy(int statusId, string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Status>> GetHomeTimeline(string maxId = null, string sinceId = null)
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
        public Task<IEnumerable<Status>> GetPublicTimeline(string maxId = null, string sinceId = null, bool local = false)
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
        public Task<IEnumerable<Status>> GetTagTimeline(string hashtag, string maxId = null, string sinceId = null, bool local = false)
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
            string url = "/api/v1/timelines/tag" + hashtag;

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

        public TimelineStreaming GetPublicStreaming()
        {
            return GetPublicStreaming(this.Instance);
        }

        public TimelineStreaming GetPublicStreaming(string instance)
        {
            string url = "https://" + instance + "/api/v1/streaming/public";

            return new TimelineStreaming(url, AccessToken);
        }

        public TimelineStreaming GetUserStreaming()
        {
            return GetUserStreaming(this.Instance);
        }

        public TimelineStreaming GetUserStreaming(string instance)
        {
            string url = "https://" + instance + "/api/v1/streaming/user";

            return new TimelineStreaming(url, AccessToken);
        }

        public TimelineStreaming GetHashtagStreaming(string hashtag)
        {
            return GetHashtagStreaming(this.Instance, hashtag);
        }

        public TimelineStreaming GetHashtagStreaming(string instance, string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", "hashtag");
            }

            string url = "https://" + instance + "/api/v1/streaming/hashtag?tag=" + hashtag;

            return new TimelineStreaming(url, AccessToken);
        }

        #endregion

    }
}
