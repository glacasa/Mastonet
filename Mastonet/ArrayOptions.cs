using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mastonet
{
    public class ArrayOptions
    {
        public string MaxId { get; set; }

        public string SinceId { get; set; }

        internal string ToQueryString()
        {
            var query = new Collection<string>();
            if (!string.IsNullOrEmpty(this.MaxId))
            {
                query.Add("max_id=" + this.MaxId);
            }
            if (!string.IsNullOrEmpty(this.SinceId))
            {
                query.Add("since_id=" + this.SinceId);
            }
            return string.Join("&", query);
        }
    }
}
