using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mastonet
{
    public class AuthenticationClient : BaseHttpClient, IAuthenticationClient
    {
        public AuthenticationClient(string instance)
        {
            this.Instance = instance;
        }

        public AuthenticationClient(AppRegistration app)
        {
            this.Instance = app.Instance;
            this.AppRegistration = app;
        }

        #region Apps

        /// <summary>
        /// Registering an application
        /// </summary>
        /// <param name="instance">Instance to connect</param>
        /// <param name="appName">Name of your application</param>
        /// <param name="scope">The rights needed by your application</param>
        /// <param name="website">URL to the homepage of your app</param>
        /// <returns></returns>
        public async Task<AppRegistration> CreateApp(string appName, Scope scope, string website = null, string redirectUri = null)
        {
            var data = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("client_name", appName),
                new KeyValuePair<string, string>("scopes", GetScopeParam(scope)),
            };
            if (string.IsNullOrEmpty(redirectUri))
            {
                data.Add(new KeyValuePair<string, string>("redirect_uris", "urn:ietf:wg:oauth:2.0:oob"));
            }
            else
            {
                data.Add(new KeyValuePair<string, string>("redirect_uris", redirectUri));
            }
            if (!string.IsNullOrEmpty(website))
            {
                data.Add(new KeyValuePair<string, string>("website", website));
            }

            var appRegistration = await Post<AppRegistration>("/api/v1/apps", data);

            appRegistration.Instance = Instance;
            appRegistration.Scope = scope;
            this.AppRegistration = appRegistration;

            return appRegistration;
        }

        #endregion

        #region Auth

        public async Task<Auth> ConnectWithPassword(string email, string password)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("scope", GetScopeParam(AppRegistration.Scope)),
            };

            var auth = await Post<Auth>("/oauth/token", data);
            this.AuthToken = auth;
            return auth;
        }

        public async Task<Auth> ConnectWithCode(string code, string redirect_uri = null)
        {
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
                new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri ?? "urn:ietf:wg:oauth:2.0:oob"),
                new KeyValuePair<string, string>("code", code),
            };

            var auth = await Post<Auth>("/oauth/token", data);
            this.AuthToken = auth;
            return auth;
        }

        public string OAuthUrl(string redirectUri = null)
        {
            if (!string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = WebUtility.UrlEncode(WebUtility.UrlDecode(redirectUri));
            }
            else
            {
                redirectUri = "urn:ietf:wg:oauth:2.0:oob";
            }

            return $"https://{this.Instance}/oauth/authorize?response_type=code&client_id={this.AppRegistration.ClientId}&scope={GetScopeParam(AppRegistration.Scope).Replace(" ", "%20")}&redirect_uri={redirectUri ?? "urn:ietf:wg:oauth:2.0:oob"}";
        }

        private static string GetScopeParam(Scope scope)
        {
            var scopeParam = "";
            if ((scope & Scope.Read) == Scope.Read) scopeParam += " read";
            if ((scope & Scope.Write) == Scope.Write) scopeParam += " write";
            if ((scope & Scope.Follow) == Scope.Follow) scopeParam += " follow";

            return scopeParam.Trim();
        }

        #endregion
    }
}
