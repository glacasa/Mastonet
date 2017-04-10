using Mastonet.Entities;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace Mastonet.Tests
{
    public class UnitTest1
    {
        AppRegistration app = new AppRegistration
        {
            ClientId = "d1c37228a5fca33791463e2d6689cd9532b280bc7d6af15f6d0fb470391e300b",
            ClientSecret = "99f0b06d509d74f22453188b43b4eb232dece8912489b715935cc18ccc4b975d"
        };
        Auth auth = new Auth
        {
            AccessToken = "0393c8aa16b3263558031c742806c57179e34391b7f0d135b878782206776e24"
        };

        [Fact]
        public async Task GetAccount()
        {
            var client = new MastodonClient("mastodon.social", app, auth);

            //await client.RegisterApp("MastonetTest");
            //await client.Connect("mastodon@adhess.net", "");

            var account = await client.GetAccount(33049);


            Assert.Equal("glacasa", account.UserName);
            Assert.Equal("Guillaume Lacasa", account.DisplayName);
        }

        [Fact]
        public async Task GetCurrentUser()
        {
            var client = new MastodonClient("mastodon.social", app, auth);

            var account = await client.GetCurrentUser();

            Assert.Equal("glacasa", account.UserName);
            Assert.Equal("Guillaume Lacasa", account.DisplayName);
        }

        [Fact]
        public async Task GetFollowing()
        {
            var client = new MastodonClient("mastodon.social", app, auth);

            var accounts = await client.GetAccountFollowing(33049);

            Assert.Equal(1, accounts.Count());
        }

        [Fact]
        public async Task GetFollowers()
        {
            var client = new MastodonClient("mastodon.social", app, auth);

            var accounts = await client.GetAccountFollowers(33049);

            Assert.Equal(1, accounts.Count());
        }
        [Fact]
        public async Task GetStatuses()
        {
            var client = new MastodonClient("mastodon.social", app, auth);

            var statuses = await client.GetAccountStatuses(33049);
            Assert.Equal(1, statuses.Count());

            statuses = await client.GetAccountStatuses(33049, onlyMedia: true);
            Assert.Equal(0, statuses.Count());
        }

    }
}
