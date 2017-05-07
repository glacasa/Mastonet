using System;
using System.Linq;
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
            var client = GetTestClient();
            var rel = await client.Block(10);
            Assert.NotNull(rel);
            Assert.True(rel.Blocking);
        }

        [Fact]
        public async Task Unblock()
        {
            var client = GetTestClient();
            var rel = await client.Unblock(10);
            Assert.NotNull(rel);
            Assert.False(rel.Blocking);
        }

        [Fact]
        public async Task GetBlocks()
        {
            var client = GetTestClient();
            var blocked = await client.GetBlocks();
            Assert.NotNull(blocked);
        }

        [Fact]
        public async Task Mute()
        {
            var client = GetTestClient();
            var rel = await client.Mute(10);
            Assert.NotNull(rel);
            Assert.True(rel.Muting);
        }

        [Fact]
        public async Task Unmute()
        {
            var client = GetTestClient();
            var rel = await client.Unmute(10);
            Assert.NotNull(rel);
            Assert.False(rel.Muting);
        }

        [Fact]
        public async Task GetMutes()
        {
            var client = GetTestClient();
            var muted = await client.GetMutes();
            Assert.NotNull(muted);
        }

        [Fact]
        public async Task FavouriteUnfavourite()
        {
            var client = GetTestClient();
            var tl = await client.GetHomeTimeline(limit : 1);
            var status = tl.First();

            status = await client.Favourite(status.Id);
            Assert.True(status.Favourited);
            
            status = await client.Unfavourite(status.Id);
            Assert.False(status.Favourited);
        }


        [Fact]
        public async Task GetFavourites()
        {
            var client = GetTestClient();
            var favs = await client.GetFavourites();
            Assert.NotNull(favs);
        }
    }
}
