using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Mastonet;

/// <summary>
/// Singleton-like static class for <see cref="System.Net.Http.HttpClient"/>.
/// Used as default in constructor parameter of <see cref="Mastonet.BaseHttpClient"/> and its subclasses.
/// </summary>
internal static class DefaultHttpClient
{
    /// <summary>
    /// The only <see cref="System.Net.Http.HttpClient"/> instance.
    /// </summary>
    internal static HttpClient Instance { get; } = new HttpClient();
}
