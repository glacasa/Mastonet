using System;

namespace Mastonet;

public class RateLimitEventArgs : EventArgs
{
    public ApiCallCategory RateLimitCategory { get; internal set; }
    public int Limit { get; internal set; }
    public int Remaining { get; internal set; }
    public DateTime Reset { get; internal set; }
}


public enum ApiCallCategory
{
    Global,
    MediaUpload,
    StatusDelete
}