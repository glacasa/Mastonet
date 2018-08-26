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
            var client = GetTestClient();

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
            var client = GetTestClient();

            var status = await client.PostStatus("Yo", Visibility.Private);

            status = await client.GetStatus(status.Id);
            Assert.False(status.Favourited);
            Assert.False(status.MediaAttachments.Any());
            Assert.Equal("TestAccount", status.Account.AccountName);
            Assert.Equal(Visibility.Private, status.Visibility);
        }

        [Fact]
        public async Task GetStatusContext()
        {
            var client = GetTestClient();

            var status = await client.PostStatus("Yo", Visibility.Private);
            var status2 = await client.PostStatus("Yo 2", Visibility.Private, replyStatusId: status.Id);

            var context = await client.GetStatusContext(status2.Id);
            Assert.Single(context.Ancestors);
            Assert.Empty(context.Descendants);
        }

        [Fact]
        public async Task GetStatusCard()
        {
            var client = GetTestClient();

            var status = await client.PostStatus("Yo", Visibility.Private);

            var card = await client.GetStatusCard(status.Id);
        }

        [Fact]
        public async Task GetRebloggedBy()
        {
            var testClient = GetTestClient();
            var privateClient = GetPrivateClient();

            var status = await testClient.PostStatus("Yo", Visibility.Public);
            await privateClient.Reblog(status.Id);

            var rbBy = await privateClient.GetRebloggedBy(status.Id);
            Assert.NotNull(rbBy.FirstOrDefault(a => a.Id == 11));
        }

        [Fact]
        public async Task GetFavouritedBy()
        {
            var testClient = GetTestClient();
            var privateClient = GetPrivateClient();

            var status = await testClient.PostStatus("Yo", Visibility.Public);
            await privateClient.Favourite(status.Id);

            var rbBy = await privateClient.GetFavouritedBy(status.Id);
            Assert.NotNull(rbBy.FirstOrDefault(a => a.Id == 11));
        }
    }
}
