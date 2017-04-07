using System;
using System.Threading.Tasks;
using Nestodon.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

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
    }
}
