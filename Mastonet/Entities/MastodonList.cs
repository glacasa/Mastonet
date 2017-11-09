using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class MastodonList<T> : List<T>
    {
        public long? NextPageMaxId { get; internal set; }
        public long? PreviousPageSinceId { get; internal set; }
    }
}
