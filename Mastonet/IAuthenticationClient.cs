using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mastonet;

public interface IAuthenticationClient
{
    string Instance { get; }

    /// <summary>
    /// Registering an application
    /// </summary>
    /// <param name="instance">Instance to connect</param>
    /// <param name="appName">Name of your application</param>
    /// <param name="scope">The rights needed by your application</param>
    /// <param name="website">URL to the homepage of your app</param>
    /// <returns></returns>
    [Obsolete("Use GranularScopes instead of deprecated Scope")]
    Task<AppRegistration> CreateApp(string appName, Scope scope, string? website = null, string? redirectUri = null);

    Task<AppRegistration> CreateApp(string appName, string? website = null, string? redirectUri = null, params GranularScope[] scope);
    Task<AppRegistration> CreateApp(string appName, string? website = null, string? redirectUri = null, IEnumerable<GranularScope>? scope = null);

    Task<Auth> ConnectWithPassword(string email, string password);

    Task<Auth> ConnectWithCode(string code, string? redirect_uri = null);

    Task Revoke(string token);

    string OAuthUrl(string? redirectUri = null);
}
