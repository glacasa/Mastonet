using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mastonet
{
    public class ArrayOptions
    {
        public int? MaxId { get; set; }

        public int? SinceId { get; set; }

        internal string ToQueryString()
        {
            var query = new Collection<string>();
            if (this.MaxId.HasValue)
            {
                query.Add("max_id=" + this.MaxId);
            }
            if (this.SinceId.HasValue)
            {
                query.Add("since_id=" + this.SinceId);
            }
            return string.Join("&", query);
        }
    }
}
