using System;
using System.Threading.Tasks;
using Mastonet.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Mastonet
{
    partial class MastodonClient
    {
        #region Follow
        /// <summary>
        /// Following an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Relationship> Follow(long accountId)
        {
            return this.Post<Relationship>($"/api/v1/accounts/{accountId}/follow");
        }

        /// <summary>
        /// Unfollowing an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Relationship> Unfollow(long accountId)
        {
            return this.Post<Relationship>($"/api/v1/accounts/{accountId}/unfollow");
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
            return this.Post<Account>($"/api/v1/follows", data);
        }
        #endregion

        #region Block
        /// <summary>
        /// Blocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Relationship> Block(long accountId)
        {
            return Post<Relationship>($"/api/v1/accounts/{accountId}/block");
        }

        /// <summary>
        /// Unblocking an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Relationship> Unblock(long accountId)
        {
            return Post<Relationship>($"/api/v1/accounts/{accountId}/unblock");
        }

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        public Task<MastodonList<Account>> GetBlocks(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetBlocks(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts blocked by the authenticated user</returns>
        public Task<MastodonList<Account>> GetBlocks(ArrayOptions options)
        {
            var url = "/api/v1/blocks";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Account>(url);
        }
        #endregion

        #region Mutes
        /// <summary>
        /// Muting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Relationship> Mute(long accountId)
        {
            return Post<Relationship>($"/api/v1/accounts/{accountId}/mute");
        }

        /// <summary>
        /// Unmuting an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns the target Account</returns>
        public Task<Relationship> Unmute(long accountId)
        {
            return Post<Relationship>($"/api/v1/accounts/{accountId}/unmute");
        }

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        public Task<MastodonList<Account>> GetMutes(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetMutes(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts muted by the authenticated user</returns>
        public Task<MastodonList<Account>> GetMutes(ArrayOptions options)
        {
            var url = "/api/v1/mutes";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Account>(url);
        }
        #endregion

        #region Domain blocks
        /// <summary>
        /// Fetching a user's blocked domains
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of strings</returns>
        public Task<MastodonList<string>> GetDomainBlocks(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetDomainBlocks(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's blocked domains
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of strings</returns>
        public Task<MastodonList<string>> GetDomainBlocks(ArrayOptions options)
        {
            var url = "/api/v1/domain_blocks";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<string>(url);
        }

        /// <summary>
        /// Block a domain
        /// </summary>
        /// <param name="domain">Domain to block</param>
        public Task BlockDomain(string domain)
        {
            var url = "/api/v1/domain_blocks?domain=" + Uri.EscapeUriString(domain);
            return Post(url);
        }

        /// <summary>
        /// Unblock a domain
        /// </summary>
        /// <param name="domain">Domain to block</param>
        public Task UnblockDomain(string domain)
        {
            var url = "/api/v1/domain_blocks?domain=" + Uri.EscapeUriString(domain);
            return Delete(url);
        }

        #endregion

    }
}
