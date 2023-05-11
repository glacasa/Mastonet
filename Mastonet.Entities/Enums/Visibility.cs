using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

public class VisibilityConverter : JsonConverter<Visibility>
{
    public override Visibility Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();

        var result = Enum.Parse(typeof(Visibility), val, true);

        return (Visibility) result;
    }

    public override void Write(Utf8JsonWriter writer, Visibility value, JsonSerializerOptions options)
    {
        var val = value.ToString();
        writer.WriteStringValue(val);
    }
}