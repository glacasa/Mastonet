using Mastonet.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class MastodonClientTests
    {
        static string instance = "mastodon.social";

        static AppRegistration app = new AppRegistration
        {
            ClientId = "d1c37228a5fca33791463e2d6689cd9532b280bc7d6af15f6d0fb470391e300b",
            ClientSecret = "99f0b06d509d74f22453188b43b4eb232dece8912489b715935cc18ccc4b975d"
        };
        static Auth auth = new Auth
        {
            AccessToken = "0393c8aa16b3263558031c742806c57179e34391b7f0d135b878782206776e24"
        };

        protected MastodonClient GetReadClient()
        {
            return new MastodonClient(instance, app, auth);
        }

        protected MastodonClient GetWriteClient()
        {
            return new MastodonClient(instance, app, auth);
        }

        protected MastodonClient GetFollowClient()
        {
            return new MastodonClient(instance, app, auth);
        }
        

    }
}
