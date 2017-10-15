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
        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses favourited by the authenticated user</returns>
        public Task<IEnumerable<Status>> GetFavourites(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetFavourites(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
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

    }
}
