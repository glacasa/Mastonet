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
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetHomeTimeline(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return GetHomeTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit });
        }

        /// <summary>
        /// Retrieving Home timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetHomeTimeline(ArrayOptions options)
        {
            string url = "/api/v1/timelines/home";
            if (options != null)
            {
                url += "?" + options.ToQueryString();
            }
            return GetList<Status>(url);
        }

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetPublicTimeline(long? maxId = null, long? sinceId = null, int? limit = null, bool local = false)
        {
            return GetPublicTimeline(new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, local);
        }

        /// <summary>
        /// Retrieving Public timeline
        /// </summary>
        /// <param name="options">Define the first and last items to get</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetPublicTimeline(ArrayOptions options, bool local = false)
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

            return GetList<Status>(url + queryParams);
        }

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="maxId">Get items with ID less than or equal this value</param>
        /// <param name="sinceId">Get items with ID greater than this value</param>
        /// <param name="limit ">Maximum number of items to get (Default 40, Max 80)</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetTagTimeline(string hashtag, long? maxId = null, long? sinceId = null, int? limit = null, bool local = false)
        {
            return GetTagTimeline(hashtag, new ArrayOptions() { MaxId = maxId, SinceId = sinceId, Limit = limit }, local);
        }

        /// <summary>
        /// Retrieving Tag timeline
        /// </summary>
        /// <param name="hashtag">The tag to retieve</param>
        /// <param name="local">Only return statuses originating from this instance</param>
        /// <param name="options">Define the first and last items to get</param>
        /// <returns>Returns an array of Statuses, most recent ones first</returns>
        public Task<MastodonList<Status>> GetTagTimeline(string hashtag, ArrayOptions options, bool local = false)
        {
            string url = "/api/v1/timelines/tag/" + hashtag;

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

            return GetList<Status>(url + queryParams);
        }



        #region Streaming

        private string StreamingApiUrl
        {
            get
            {
                // mstdn.jp uses a different url for its streaming API, althoug it's supposed to be a dev feature.
                // if other instances have the same problem, they will be added there, until the API is updated to
                // allow us get the correct url
                switch (this.Instance)
                {
                    case "mstdn.jp":
                        return "streaming.mstdn.jp";
                    default:
                        return Instance;
                }
            }
        }

        public TimelineStreaming GetPublicStreaming()
        {
            string url = "https://" + StreamingApiUrl + "/api/v1/streaming/public";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        public TimelineStreaming GetPublicStreaming(string streamingApiUrl)
        {
            string url = "https://" + streamingApiUrl + "/api/v1/streaming/public";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        public TimelineStreaming GetUserStreaming()
        {
            string url = "https://" + StreamingApiUrl + "/api/v1/streaming/user";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        public TimelineStreaming GetUserStreaming(string streamingApiUrl)
        {
            string url = "https://" + streamingApiUrl + "/api/v1/streaming/user";

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        public TimelineStreaming GetHashtagStreaming(string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", "hashtag");
            }

            string url = "https://" + StreamingApiUrl + "/api/v1/streaming/hashtag?tag=" + hashtag;

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        [Obsolete("Only use this method if the instance has a different streaming url. Please report the instance name here to allow us to support it : https://github.com/glacasa/Mastonet/issues/10")]
        public TimelineStreaming GetHashtagStreaming(string streamingApiUrl, string hashtag)
        {
            if (string.IsNullOrEmpty(hashtag))
            {
                throw new ArgumentException("You must specify a hashtag", "hashtag");
            }

            string url = "https://" + streamingApiUrl + "/api/v1/streaming/hashtag?tag=" + hashtag;

            return new TimelineStreaming(url, AuthToken.AccessToken);
        }

        #endregion
    }
}
