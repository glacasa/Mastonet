using System;
using System.Collections.Generic;
using System.Text;
using Mastonet.Entities;
using System.Threading.Tasks;

namespace Mastonet.Compat
{
    [Obsolete("Methods have changed")]
    public class MastodonClient : Mastonet.MastodonClient
    {
        #region Ctor

        public MastodonClient(AppRegistration appRegistration, Auth accessToken) 
            : base(appRegistration, accessToken)
        {
        }

        #endregion
        
        public new async Task<IEnumerable<Status>> GetHomeTimeline(long? maxId = null, long? sinceId = null, int? limit = null)
        {
            return (await base.GetHomeTimeline(maxId, sinceId, limit)).Items;
        }

        public new async Task<IEnumerable<Status>> GetHomeTimeline(ArrayOptions options)
        {
            return (await base.GetHomeTimeline(options)).Items;
        }

        public new async Task<IEnumerable<Status>> GetPublicTimeline(long? maxId = null, long? sinceId = null, int? limit = null, bool local = false)
        {
            return (await base.GetPublicTimeline(maxId, sinceId, limit)).Items;
        }

        public new async Task<IEnumerable<Status>> GetPublicTimeline(ArrayOptions options, bool local = false)
        {
            return (await base.GetPublicTimeline(options)).Items;
        }

    }
}
