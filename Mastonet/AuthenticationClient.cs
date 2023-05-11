using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mastonet;

public class AuthenticationClient : BaseHttpClient, IAuthenticationClient
{
    public AppRegistration? AppRegistration { get; set; }

    public AuthenticationClient(string instance) : this(instance, DefaultHttpClient.Instance) { }
    public AuthenticationClient(AppRegistration app) : this(app, DefaultHttpClient.Instance) { }

    public AuthenticationClient(string instance, HttpClient client) : base(client)
    {
        this.Instance = instance;
    }

    public AuthenticationClient(AppRegistration app, HttpClient client) : base(client)
    {
        this.Instance = app.Instance;
        this.AppRegistration = app;
    }

    #region Apps


    public Task<AppRegistration> CreateApp(string appName, string? website = null, string? redirectUri = null, params GranularScope[] scope)
    {
        return CreateApp(appName, website, redirectUri, scope.AsEnumerable());
    }

    public async Task<AppRegistration> CreateApp(string appName, string? website = null, string? redirectUri = null, IEnumerable<GranularScope>? scope = null)
    {
        var scopeString = GetScopeParam(scope);
        var data = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("client_name", appName),
            new KeyValuePair<string, string>("scopes", scopeString),
            new KeyValuePair<string, string>("redirect_uris", redirectUri?? "urn:ietf:wg:oauth:2.0:oob")
        };

        if (website != null)
        {
            data.Add(new KeyValuePair<string, string>("website", website));
        }

        var appRegistration = await Post<AppRegistration>("/api/v1/apps", data);

        appRegistration.Instance = Instance;
        appRegistration.Scope = scopeString;
        this.AppRegistration = appRegistration;

        return appRegistration;
    }

    #endregion

    #region Auth

    public Task<Auth> ConnectWithPassword(string email, string password)
    {
        if (AppRegistration == null)
        {
            throw new InvalidOperationException("The app must be registered before you can connect");
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
            new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", email),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("scope", AppRegistration.Scope),
        };

        return Post<Auth>("/oauth/token", data);
    }

    public Task<Auth> ConnectWithCode(string code, string? redirect_uri = null)
    {
        if (AppRegistration == null)
        {
            throw new InvalidOperationException("The app must be registered before you can connect");
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
            new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("redirect_uri", redirect_uri ?? "urn:ietf:wg:oauth:2.0:oob"),
            new KeyValuePair<string, string>("code", code),
        };

        return Post<Auth>("/oauth/token", data);
    }

    public string OAuthUrl(string? redirectUri = null)
    {
        if (AppRegistration == null)
        {
            throw new InvalidOperationException("The app must be registered before you can connect");
        }

        if (redirectUri != null)
        {
            redirectUri = WebUtility.UrlEncode(WebUtility.UrlDecode(redirectUri));
        }
        else
        {
            redirectUri = "urn:ietf:wg:oauth:2.0:oob";
        }

        return $"https://{this.Instance}/oauth/authorize?response_type=code&client_id={AppRegistration.ClientId}&scope={AppRegistration.Scope.Replace(" ", "%20")}&redirect_uri={redirectUri ?? "urn:ietf:wg:oauth:2.0:oob"}";
    }

    /// <summary>
    /// Revoke an access token to make it no longer valid for use.
    /// </summary>
    /// <param name="token">The previously obtained token, to be invalidated.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public Task Revoke(string token)
    {
        if (AppRegistration == null)
        {
            throw new InvalidOperationException("You need to revoke a token with the app CclientId and ClientSecret used to obtain the Token");
        }

        var data = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("client_id", AppRegistration.ClientId),
            new KeyValuePair<string, string>("client_secret", AppRegistration.ClientSecret),
            new KeyValuePair<string, string>("token", token),
        };

        return Post<Auth>("/oauth/revoke", data);
    }

    private static string GetScopeParam(IEnumerable<GranularScope>? scopes)
    {
        if (scopes == null)
        {
            return "";
        }

        return String.Join(" ", scopes.Select(s => s.ToString().ToLowerInvariant().Replace("__", ":")));
    }

    #endregion

    protected override void OnResponseReceived(HttpResponseMessage response)
    {
    }
}
