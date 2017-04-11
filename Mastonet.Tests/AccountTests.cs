using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mastonet.Tests
{
    public class AccountTests : MastodonClientTests
    {
        [Fact]
        public async Task GetAccount()
        {
            var client = GetReadClient();

            var account = await client.GetAccount(1);

            Assert.NotNull(account.ProfileUrl);
            Assert.NotNull(account.UserName);
        }

        [Fact]
        public async Task GetCurrentUser()
        {
            var client = GetReadClient();

            var account = await client.GetCurrentUser();

            Assert.NotNull(account.ProfileUrl);
            Assert.NotNull(account.UserName);
        }

        [Fact]
        public async Task GetAccountRelationships()
        {
            var client = GetReadClient();

            var relationships = await client.GetAccountRelationships(1);

            Assert.NotNull(relationships);
            Assert.NotEqual(0, relationships.Count());
        }
    }
}
