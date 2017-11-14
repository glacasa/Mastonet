using Mastonet;
using Mastonet.Entities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace _InitTest
{
    public class InitTest
    {
        // Add instance name, email and password of a test account, and run this test to get authentication tokens
        // The test will throw an Exception with the token data, just copy and paste it on the class below

        [Fact]
        public async Task GetTokens()
        {
            var authClient = new AuthenticationClient(Instance);
            var appRegistration = await authClient.CreateApp("Mastonet Unit Test", Scope.Read | Scope.Write | Scope.Follow);

            var accessToken1 = await authClient.ConnectWithPassword(Email1, Password1);
            var accessToken2 = await authClient.ConnectWithPassword(Email2, Password2);


            var authCodeBuilder = new StringBuilder();
            authCodeBuilder.AppendLine("private static AppRegistration app = new AppRegistration");
            authCodeBuilder.AppendLine("{");
            authCodeBuilder.AppendLine("Instance = \"" + Instance + "\",");
            authCodeBuilder.AppendLine("ClientId = \"" + appRegistration.ClientId + "\",");
            authCodeBuilder.AppendLine("ClientSecret = \"" + appRegistration.ClientSecret + "\"");
            authCodeBuilder.AppendLine("};");
            authCodeBuilder.AppendLine("private static string testAccessToken = \"" + accessToken1.AccessToken + "\";");
            authCodeBuilder.AppendLine("private static string privateAccessToken = \"" + accessToken2.AccessToken + "\";");

            var authCode = authCodeBuilder.ToString();

            throw new Exception(authCode);
        }

        private string Instance => "";

        private string Email1 => "";
        private string Password1 => "";

        private string Email2 => "";
        private string Password2 => "";
    }
}



namespace Mastonet.Tests
{
    public class MastodonClientTests
    {
        //####################
        //##  REPLACE HERE  ##
        //####################
        private static AppRegistration app = new AppRegistration
        {
            Instance = "",
            ClientId = "",
            ClientSecret = ""
        };
        private static string testAccessToken = "";
        private static string privateAccessToken = "";
        //####################
        //####################


        protected MastodonClient GetTestClient()
        {
            return new MastodonClient(app, new Auth() { AccessToken = testAccessToken });
        }


        protected MastodonClient GetPrivateClient()
        {
            return new MastodonClient(app, new Auth() { AccessToken = privateAccessToken });
        }

    }
}
