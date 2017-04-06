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
            var client = new NestodonClient("mamot.fr", "ecb051ae66b6989112afc496153f6141bd0bd00398c3c189aaad406f69b93a8b", "a38eb97955d84e6b5fde759dc2c6a9d277783c4774686846f8c07a664102b528");

            //await client.RegisterApp("NestodonTest");
            //await client.Connect("mastodon@adhess.net", "");

            //var account = await client.GetAccount("glacasa");


            //Assert.Equal("gltesting", account.UserName);
            //Assert.Equal("", account.DisplayName);
        }
    }
}
