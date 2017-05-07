using Mastonet.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class MastodonClientTests
    {
        private static AppRegistration app = new AppRegistration
        {
            Instance = "mastonet.masto.host",
            ClientId = "5713e1fe569b5486dde452f3107a1696a38341f14b75f682c6f4cfc8faa5d8c6",
            ClientSecret = "0bea18af2d99037908b6709ff1cc2ebe1762127f520051a461da64836f2ec6ff"
        };

        private static string testAccessToken = "9843b3248aa210fd6156218ce2384fdf84ee7f555e158b835c596ebdd1b89952";

        protected MastodonClient GetTestClient()
        {
            return new MastodonClient(app, new Auth() { AccessToken = testAccessToken });
        }

        

        private static string privateAccessToken = "c9ba4e2788ae3d756f54de22083dad7fcde659003646e99664d4fe3b163abf94";

        protected MastodonClient GetPrivateClient()
        {
            return new MastodonClient(app, new Auth() { AccessToken = privateAccessToken });
        }


    }
}
