using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class SearchTests : MastodonClientTests
    {
        [Fact]
        public async Task SearchAccounts()
        {
            var client = GetTestClient();

            var found = await client.SearchAccounts("glacasa");
            Assert.True(found.Any());
        }

        [Fact]
        public async Task Search()
        {
            var client = GetTestClient();
            var found = await client.Search("search", false);

            Assert.True(found.Hashtags.Any());
        }
    }
}
