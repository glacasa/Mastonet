using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class MediaTests : MastodonClientTests
    {
        [Fact]
        public async Task UploadPngImage()
        {
            var client = GetWriteClient();

            System.IO.FileStream fs = new System.IO.FileStream(
                                                @".\testimage.png",
                                                System.IO.FileMode.Open,
                                                System.IO.FileAccess.Read);
            var attachment = await client.UploadMedia(fs, "testimage.png");
            fs.Dispose();

            Assert.NotNull(attachment);
            Assert.NotNull(attachment.PreviewUrl);
            Assert.NotNull(attachment.Url);
        }
    }
}
