using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet;

/// <summary>
/// Represents the visibility of a status
/// </summary>
public enum Visibility
{
    /// <summary>
    /// Visible to everyone, shown in public timelines.
    /// </summary>
    Public,

    /// <summary>
    /// Visible to public, but not included in public timelines.
    /// </summary>
    Unlisted,

    /// <summary>
    /// Visible to followers only, and to any mentioned users.
    /// </summary>
    Private,

    /// <summary>
    /// Visible only to mentioned users.
    /// </summary>
    Direct,
}
