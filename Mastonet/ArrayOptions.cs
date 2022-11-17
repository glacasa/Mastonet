using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mastonet;

public class ArrayOptions
{
    public string? MaxId { get; set; }

    public string? SinceId { get; set; }

    public string? MinId { get; set; }

    public int? Limit { get; set; }

    internal string ToQueryString()
    {
        var query = new Collection<string>();
        if (!string.IsNullOrEmpty(MaxId))
        {
            query.Add("max_id=" + MaxId);
        }
        if (!string.IsNullOrEmpty(SinceId))
        {
            query.Add("since_id=" + SinceId);
        }
        if (!string.IsNullOrEmpty(MinId))
        {
            query.Add("min_id=" + MinId);
        }
        if (Limit.HasValue)
        {
            query.Add("limit=" + Limit);
        }
        return string.Join("&", query);
    }
}
