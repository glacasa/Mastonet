using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class MastodonList<T> : List<T>
    {
        public long NextPageSinceID { get; internal set; }
        public long PreviousPageMaxID { get; internal set; }
    }
}
