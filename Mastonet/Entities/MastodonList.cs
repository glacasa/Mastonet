using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities
{
    public class MastodonList<T>
    {
        public IEnumerable<T> Items { get; internal set; }

        public long NextPageSinceId { get; internal set; }
        public long PreviousPageMaxId { get; internal set; }
    }
}
