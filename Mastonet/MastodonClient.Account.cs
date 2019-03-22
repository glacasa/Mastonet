using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mastonet
{
    partial class MastodonClient
    {
        /// <summary>
        /// Fetching an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>Returns an Account</returns>
        public Task<Account> GetAccount(long accountId)
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
        /// Updating the current user
        /// </summary>
        /// <param name="display_name">The name to display in the user's profile</param>
        /// <param name="note">A new biography for the user</param>
        /// <param name="avatar">A base64 encoded image to display as the user's avatar</param>
        /// <param name="header">A base64 encoded image to display as the user's header image</param>
        /// <returns>Returns the authenticated user's Account</returns>
        public Task<Account> UpdateCredentials(string display_name = null, string note = null, MediaDefinition avatar = null, MediaDefinition header = null)
        {
            var data = new List<KeyValuePair<string, string>>();
            var media = new List<MediaDefinition>();

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
                avatar.ParamName = "avatar";
                media.Add(avatar);
            }
            if (header != null)
            {
                header.ParamName = "header";
                media.Add(header);
            }

            return Patch<Account>($"/api/v1/accounts/update_credentials", data, media);
        }

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Returns an array of Relationships of the current user to a given account</returns>
        public Task<IEnumerable<Relationship>> GetAccountRelationships(long id)
        {
            return GetAccountRelationships(new long[] { id });
        }

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <param name="id">Account IDs</param>
        /// <returns>Returns an array of Relationships of the current user to a list of given accounts</returns>
        public Task<IEnumerable<Relationship>> GetAccountRelationships(IEnumerable<long> ids)
        {
            var data = new List<KeyValuePair<string, string>>();
            foreach (var id in ids)
            {
                data.Add(new KeyValuePair<string, string>("id[]", id.ToString()));
            }
            return Get<IEnumerable<Relationship>>("/api/v1/accounts/relationships", data);
        }

        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>        
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetAccountFollowers(long accountId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetAccountFollowers(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }
        
        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetAccountFollowers(long accountId, ArrayOptions options)
        {
            var url = $"/api/v1/accounts/{accountId}/followers";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetAccountFollowing(long accountId, long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetAccountFollowing(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts</returns>
        public Task<MastodonList<Account>> GetAccountFollowing(long accountId, ArrayOptions options)
        {
            var url = $"/api/v1/accounts/{accountId}/following";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses</returns>
        public Task<MastodonList<Status>> GetAccountStatuses(long accountId, long? maxId = null, long? sinceId = null, int? limit = null, bool onlyMedia = false, bool excludeReplies = false)
        {
            return GetAccountStatuses(accountId, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, onlyMedia, excludeReplies);
        }

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses</returns>
        public Task<MastodonList<Status>> GetAccountStatuses(long accountId, ArrayOptions options, bool onlyMedia = false, bool excludeReplies = false)
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

            return GetMastodonList<Status>(url + queryParams);
        }
    
        
         #region Follow Requests

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        public Task<MastodonList<Account>> GetFollowRequests(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetFollowRequests(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Accounts which have requested to follow the authenticated user</returns>
        public Task<MastodonList<Account>> GetFollowRequests(ArrayOptions options)
        {
            var url = "/api/v1/follow_requests";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return this.GetMastodonList<Account>(url);
        }

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to authorize</param>
        public Task AuthorizeRequest(long accountId)
        {
            return this.Post($"/api/v1/follow_requests/{accountId}/authorize");
        }

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <param name="accountId">The id of the account to reject</param>
        public Task RejectRequest(long accountId)
        {
            return this.Post($"/api/v1/follow_requests/{accountId}/reject");
        }

        #endregion

        #region Favourites

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        public Task<MastodonList<Status>> GetFavourites(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetFavourites(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        public Task<MastodonList<Status>> GetFavourites(ArrayOptions options)
        {
            var url = "/api/v1/favourites";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetMastodonList<Status>(url);
        }

        #endregion
    }
}
