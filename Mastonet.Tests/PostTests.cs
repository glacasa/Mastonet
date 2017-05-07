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

            var status = await client.PostStatus("Status with imate", Visibility.Private, mediaIds: new int[] { attachment.Id });
            status = await client.GetStatus(status.Id);

            Assert.NotNull(status.MediaAttachments);
            Assert.True(status.MediaAttachments.Any());
            Assert.Equal(attachment.Url, status.MediaAttachments.First().Url);
        }

        [Fact]
        public async Task PostStatus()
        {
            var client = GetTestClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task DeleteStatus()
        {
            var client = GetTestClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Reblog()
        {
            var client = GetTestClient();
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Unreblog()
        {
            var client = GetTestClient();
            throw new NotImplementedException();
        }
    }
}
