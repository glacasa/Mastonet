using Mastonet.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class MastodonClientTests
    {
        private static AppRegistration testApp = new AppRegistration
        {
            Instance = "mastonet.masto.host",
            ClientId = "5713e1fe569b5486dde452f3107a1696a38341f14b75f682c6f4cfc8faa5d8c6",
            ClientSecret = "0bea18af2d99037908b6709ff1cc2ebe1762127f520051a461da64836f2ec6ff"
        };

        private static string testAccessToken = "9843b3248aa210fd6156218ce2384fdf84ee7f555e158b835c596ebdd1b89952";

        protected MastodonClient GetTestClient()
        {
            return new MastodonClient(testApp, new Auth() { AccessToken = testAccessToken });
        }





        private static AppRegistration privateApp = new AppRegistration
        {
            Instance = "mastonet.masto.host",
            ClientId = "c055df68cf3e7287f1f9abe64ebaadbc7c940a56d206cbfac72855322a8411fc",
            ClientSecret = "ef5daae140f1aa360ab04d1618eb79bee9353ea0f38f71c8e21ac4f48a3f51d3"
        };

        private static string privateAccessToken = "89b4b23dff1a92f499d4717fdc97ca20ced0e7c02c60823957b2e8e7bfcb3160";

        protected MastodonClient GetPrivateClient()
        {
            return new MastodonClient(privateApp, new Auth() { AccessToken = privateAccessToken });
        }


    }
}
