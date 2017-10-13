using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class PostTests : MastodonClientTests
    {
        [Fact]
        public async Task UploadMedia()
        {
            var client = GetTestClient();

            System.IO.FileStream fs = new System.IO.FileStream(
                                                @".\testimage.png",
                                                System.IO.FileMode.Open,
                                                System.IO.FileAccess.Read);
            var attachment = await client.UploadMedia(fs, "testimage.png");
            fs.Dispose();

            Assert.NotNull(attachment);
            Assert.NotNull(attachment.PreviewUrl);
            Assert.NotNull(attachment.Url);

            var status = await client.PostStatus("Status with imate", Visibility.Private, mediaIds: new long[] { attachment.Id });
            status = await client.GetStatus(status.Id);

            Assert.NotNull(status.MediaAttachments);
            Assert.True(status.MediaAttachments.Any());
            Assert.Equal(attachment.Url, status.MediaAttachments.First().Url);
        }

        [Fact]
        public async Task PostStatus()
        {
            var client = GetTestClient();
            var status = await client.PostStatus("Yo1", Visibility.Public);

            var client2 = GetPrivateClient();

            var statusFromApi = await client2.GetStatus(status.Id);

            Assert.Equal(status.Id, statusFromApi.Id);
            Assert.Equal(status.Content, statusFromApi.Content);
            Assert.Equal(Visibility.Public, statusFromApi.Visibility);
        }

        [Fact]
        public async Task DeleteStatus()
        {
            var client = GetTestClient();
            var status = await client.PostStatus("Yo1", Visibility.Public);
            var statusId = status.Id;

            status = await client.GetStatus(statusId);
            Assert.NotNull(status);

            await client.DeleteStatus(statusId);

            await Assert.ThrowsAsync<ServerErrorException>(async () =>
            {
                status = await client.GetStatus(statusId);
            });
        }

        [Fact]
        public async Task ReblogUnreblog()
        {
            var testClient = GetTestClient();
            var status = await testClient.PostStatus("Yo1", Visibility.Public);

            var client = GetPrivateClient();
            status = await client.GetStatus(status.Id);
            Assert.False(status.Reblogged);

            await client.Reblog(status.Id);
            status = await client.GetStatus(status.Id);
            Assert.True(status.Reblogged);


            await client.Unreblog(status.Id);
            status = await client.GetStatus(status.Id);
            Assert.False(status.Reblogged);
        }
    }
}
