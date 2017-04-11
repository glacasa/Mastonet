using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class FollowTests : MastodonClientTests
    {
        [Fact]
        public async Task GetAccountFollowers()
        {
            var client = GetReadClient();
            var accounts = await client.GetAccountFollowers(1);

            Assert.NotNull(accounts);
            Assert.True(accounts.Any());
        }

        [Fact]
        public async Task GetAccountFollowing()
        {
            var client = GetReadClient();
            var accounts = await client.GetAccountFollowing(1);

            Assert.NotNull(accounts);
            Assert.True(accounts.Any());
        }

        [Fact]
        public async Task Follow()
        {
            // Follow local
            var client = GetFollowClient();
            var followedAccount = await client.Follow(1);
            Assert.NotNull(followedAccount);

            //follow remote
            followedAccount = await client.Follow("");
            Assert.NotNull(followedAccount);
        }

        [Fact]
        public async Task Unfollow()
        {
            var client = GetFollowClient();
            var unfollowedAccount = await client.Unfollow(1);
            Assert.NotNull(unfollowedAccount);
        }


        [Fact]
        public async Task GetFollowRequests()
        {
            var client = GetFollowClient();
            var requests = await client.GetFollowRequests();
            Assert.NotNull(requests);
        }

        [Fact]
        public async Task AuthorizeRequest()
        {
            var client = GetFollowClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task RejectRequest()
        {
            var client = GetFollowClient();
            throw new NotImplementedException();
        }
    }
}
