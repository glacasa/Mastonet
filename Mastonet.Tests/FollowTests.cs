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
            var client = GetTestClient();
            var accounts = await client.GetAccountFollowers(1);

            Assert.NotNull(accounts);
            Assert.True(accounts.Any());
        }

        [Fact]
        public async Task GetAccountFollowing()
        {
            var client = GetTestClient();
            var accounts = await client.GetAccountFollowing(1);

            Assert.NotNull(accounts);
            Assert.True(accounts.Any());
        }

        [Fact]
        public async Task Follow()
        {
            var client = GetTestClient();
            // Make sure we don't follow
            await client.Unfollow(4);
            await client.Unfollow(12);

            // Follow local
            var relation = await client.Follow(4);
            Assert.NotNull(relation);
            Assert.True(relation.Following);

            //follow remote
            // Remote tests removed to avoid send test requests to random instances
            //var followedAccount = await client.Follow("");
            //Assert.NotNull(followedAccount);
            //Assert.Equal("glacasa", followedAccount.UserName);
            //relation = (await client.GetAccountRelationships(followedAccount.Id)).First();
            //Assert.True(relation.Following);
        }

        [Fact]
        public async Task Unfollow()
        {
            var client = GetTestClient();
            // Make sure we follow
            await client.Follow(4);

            var relation = await client.Unfollow(4);
            Assert.NotNull(relation);
            Assert.False(relation.Following);
        }


        [Fact]
        public async Task GetFollowRequests()
        {
            var client = GetPrivateClient();
            var requests = await client.GetFollowRequests();
            Assert.NotNull(requests);
            Assert.True(requests.Any());
        }

        [Fact]
        public async Task AuthorizeRequest()
        {
            var testClient = GetTestClient();
            var privateClient = GetPrivateClient();
                        
            // Have the test follower
            await privateClient.RejectRequest(3);
            await privateClient.Unblock(3);
            await testClient.Unfollow(11);
            await testClient.Follow(11);

            var requests = await privateClient.GetFollowRequests();
            Assert.Contains(requests, r => r.Id == 3);

            // Authorize
            await privateClient.AuthorizeRequest(3);

            // Check if it's ok
            requests = await privateClient.GetFollowRequests();
            Assert.DoesNotContain(requests, r => r.Id == 3);

            var followers = await privateClient.GetAccountFollowers(11);
            Assert.Contains(followers, f => f.Id == 3);
        }

        [Fact]
        public async Task RejectRequest()
        {
            var testClient = GetTestClient();
            var privateClient = GetPrivateClient();

            // Have the test follower
            await privateClient.AuthorizeRequest(3);
            await testClient.Unfollow(11);
            await testClient.Follow(11);

            var requests = await privateClient.GetFollowRequests();
            Assert.Contains(requests, r => r.Id == 3);

            // Authorize
            await privateClient.RejectRequest(3);

            // Check if it's ok
            requests = await privateClient.GetFollowRequests();
            Assert.DoesNotContain(requests, r => r.Id == 3);

            var followers = await privateClient.GetAccountFollowers(11);
            Assert.DoesNotContain(followers, f => f.Id == 3);
        }
    }
}
