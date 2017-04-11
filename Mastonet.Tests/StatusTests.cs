using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class StatusTests : MastodonClientTests
    {
        [Fact]
        public async Task GetAccountStatuses()
        {
            var client = GetReadClient();

            var status = await client.GetAccountStatuses(1);
            Assert.NotNull(status);
            Assert.True(status.Any());
            status = await client.GetAccountStatuses(1, onlyMedia: true);
            Assert.NotNull(status);
            Assert.True(status.Any());
            status = await client.GetAccountStatuses(1, excludeReplies: true);
            Assert.NotNull(status);
            Assert.True(status.Any());
        }

        [Fact]
        public async Task GetStatus()
        {
            var client = GetReadClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetStatusContext()
        {
            var client = GetReadClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetStatusCard()
        {
            var client = GetReadClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetRebloggedBy()
        {
            var client = GetReadClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetFavouritedBy()
        {
            var client = GetReadClient();
            throw new NotImplementedException();
        }
    }
}
