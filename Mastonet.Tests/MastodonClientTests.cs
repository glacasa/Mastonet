using Mastonet.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class MastodonClientTests
    {
        static AppRegistration app = new AppRegistration
        {
            Instance = "mastodon.social",
            ClientId = "d1c37228a5fca33791463e2d6689cd9532b280bc7d6af15f6d0fb470391e300b",
            ClientSecret = "99f0b06d509d74f22453188b43b4eb232dece8912489b715935cc18ccc4b975d"
        };
        static string AccessToken = "0393c8aa16b3263558031c742806c57179e34391b7f0d135b878782206776e24";

        protected MastodonClient GetReadClient()
        {
            return new MastodonClient(app, AccessToken);
        }

        protected MastodonClient GetWriteClient()
        {
            return new MastodonClient(app, AccessToken);
        }

        protected MastodonClient GetFollowClient()
        {
            return new MastodonClient(app, AccessToken);
        }


    }
}
