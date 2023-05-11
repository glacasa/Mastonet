using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mastonet;

public interface IAuthenticationClient
{
    string Instance { get; }

    Task<AppRegistration> CreateApp(string appName, string? website = null, string? redirectUri = null, params GranularScope[] scope);
    Task<AppRegistration> CreateApp(string appName, string? website = null, string? redirectUri = null, IEnumerable<GranularScope>? scope = null);

    Task<Auth> ConnectWithPassword(string email, string password);

    Task<Auth> ConnectWithCode(string code, string? redirect_uri = null);

    Task Revoke(string token);

    string OAuthUrl(string? redirectUri = null);
}
