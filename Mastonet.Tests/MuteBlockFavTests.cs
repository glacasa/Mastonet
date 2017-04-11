using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class MuteBlockFavTests : MastodonClientTests
    {
        [Fact]
        public async Task Block()
        {
            var client = GetFollowClient();
            var blocked = await client.Block(10);
            Assert.NotNull(blocked);
        }

        [Fact]
        public async Task Unblock()
        {
            var client = GetFollowClient();
            var unblocked = await client.Unblock(10);
            Assert.NotNull(unblocked);
        }

        [Fact]
        public async Task GetBlocks()
        {
            var client = GetReadClient();
            var blocked = await client.GetBlocks();
            Assert.NotNull(blocked);
        }

        [Fact]
        public async Task Mute()
        {
            var client = GetFollowClient();
            var muted = await client.Mute(10);
            Assert.NotNull(muted);
        }

        [Fact]
        public async Task Unmute()
        {
            var client = GetFollowClient();
            var unmuted = await client.Unmute(10);
            Assert.NotNull(unmuted);
        }

        [Fact]
        public async Task GetMutes()
        {
            var client = GetReadClient();
            var muted = await client.GetMutes();
            Assert.NotNull(muted);
        }

        [Fact]
        public async Task Favourite()
        {
            var client = GetFollowClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Unfavourite()
        {
            var client = GetFollowClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetFavourites()
        {
            var client = GetReadClient();
            var favs = await client.GetFavourites();
            Assert.NotNull(favs);
        }
    }
}
