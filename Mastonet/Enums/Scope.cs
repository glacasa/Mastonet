using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet
{
    [Flags]
    public enum Scope
    {
        Read = 1,
        Write = 2,
        Follow = 4,
    }
}
