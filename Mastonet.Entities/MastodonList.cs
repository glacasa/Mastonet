using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet.Entities;

public class MastodonList<T> : List<T>
{
    public string? NextPageMaxId { get; set; }
    public string? PreviousPageSinceId { get; set; }
    public string? PreviousPageMinId { get; set; }
}