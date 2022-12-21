using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

public class MastodonList<T> : List<T>
{
    public string? NextPageMaxId { get; internal set; }
    public string? PreviousPageSinceId { get; internal set; }
    public string? PreviousPageMinId { get; internal set; }
}
