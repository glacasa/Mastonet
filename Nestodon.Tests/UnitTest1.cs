using Nestodon.Entities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Nestodon.Tests
{
    public class UnitTest1
    {

        [Fact]
        public async Task GetAccount()
        {
            var app = new AppRegistration
            {
                ClientId = "d1c37228a5fca33791463e2d6689cd9532b280bc7d6af15f6d0fb470391e300b",
                ClientSecret = "99f0b06d509d74f22453188b43b4eb232dece8912489b715935cc18ccc4b975d"
            };
            var auth = new Auth
            {
                AccessToken = "0393c8aa16b3263558031c742806c57179e34391b7f0d135b878782206776e24"
            };

            var client = new NestodonClient("mastodon.social", app, auth);

            //await client.RegisterApp("NestodonTest");
            //await client.Connect("mastodon@adhess.net", "");

            var account = await client.GetAccount(33049);


            Assert.Equal("glacasa", account.UserName);
            Assert.Equal("Guillaume Lacasa", account.DisplayName);
        }
    }
}
