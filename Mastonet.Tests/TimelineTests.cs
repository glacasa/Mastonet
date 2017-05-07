using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Mastonet.Tests
{
    public class TimelineTests : MastodonClientTests
    {
        [Fact]
        public async Task GetHomeTimeline()
        {
            var client = GetTestClient();
            var timeline = await client.GetHomeTimeline();
            Assert.NotNull(timeline);
        }

        [Fact]
        public async Task GetPublicTimeline()
        {
            var client = GetTestClient();
            var timeline = await client.GetPublicTimeline();
            Assert.NotNull(timeline);
        }

        [Fact]
        public async Task GetTagTimeline()
        {
            var client = GetTestClient();
            var timeline = await client.GetTagTimeline("mastodon");
            Assert.NotNull(timeline);
        }
    }
}
