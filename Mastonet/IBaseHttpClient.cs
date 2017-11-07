using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet
{
    public interface IBaseHttpClient
    {
        string Instance { get; }
        AppRegistration AppRegistration { get; }
        Auth AuthToken { get; }
    }
}
