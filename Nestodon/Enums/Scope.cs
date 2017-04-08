using System;
using System.Collections.Generic;
using System.Text;

namespace Nestodon.Enums
{
    [Flags]
    public enum Scope
    {
        Read = 1,
        Write = 2,
        Follow = 4,
    }
}
