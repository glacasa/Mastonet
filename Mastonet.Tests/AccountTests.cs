using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var client = GetTestClient();

            var account = await client.GetAccount("1");

            Assert.NotNull(account.ProfileUrl);
            Assert.NotNull(account.UserName);
            Assert.Equal("glacasa",account.UserName);
        }

        [Fact]
        public async Task GetCurrentUser()
        {
            var client = GetTestClient();

            var account = await client.GetCurrentUser();

            Assert.NotNull(account.ProfileUrl);
            Assert.NotNull(account.UserName);
            Assert.Equal("TestAccount", account.UserName);
        }

        [Fact]
        public async Task GetAccountRelationships()
        {
            var client = GetTestClient();

            var relationships = await client.GetAccountRelationships("1");

            Assert.NotNull(relationships);
            Assert.Single(relationships);
        }


        [Fact]
        public async Task UpdateAccount()
        {
            throw new NotImplementedException();
            //var avatarBase64 = await DownloadBase64("");
            //var headerBase64 = await DownloadBase64("");

            //var client = GetTestClient();
            //var result = await client.UpdateCredentials("Just the Doctor", "Trust me", "data:image/jpg;base64," + avatarBase64, "data:image/jpg;base64," + headerBase64);

            //Assert.NotNull(result);
        }

        //private async Task<string> DownloadBase64(string url)
        //{
        //    var bytes = await new HttpClient().GetByteArrayAsync(url);
        //    return Convert.ToBase64String(bytes);
        //}
    }
}
