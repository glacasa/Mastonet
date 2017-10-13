using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mastonet
{
    public class ArrayOptions
    {
        public long? MaxId { get; set; }

        public long? SinceId { get; set; }

        public int? Limit { get; set; }

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
            if (this.Limit.HasValue)
            {
                query.Add("limit=" + this.Limit);
            }
            return string.Join("&", query);
        }
    }
}
