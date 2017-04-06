using System;
using System.Threading.Tasks;
using Xunit;

namespace Nestodon.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetAccount()
        {
            var client = new NestodonClient("mastodon.cx");
            var account = await client.GetAccount("glacasa");

            Assert.Equal("glacasa", account.UserName);
            Assert.Equal("Guillaume Lacasa", account.DisplayName);
        }
    }
}
