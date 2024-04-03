using System;
using System.Buffers;
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
#if NET7_0_OR_GREATER
        var valueLength = reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;

        var buffer = ArrayPool<char>.Shared.Rent(valueLength);
        var charsRead = reader.CopyString(buffer);
        var val = buffer.AsSpan(0, charsRead);
        var result = Enum.Parse(typeof(Visibility), val, true);
        ArrayPool<char>.Shared.Return(buffer, clearArray: true);
#else
        var val = reader.GetString()!;
        var result = Enum.Parse(typeof(Visibility), val, true);
#endif

        return (Visibility)result;
    }

    public override void Write(Utf8JsonWriter writer, Visibility value, JsonSerializerOptions options)
    {
        switch(value)
        {
            case Visibility.Public: writer.WriteStringValue(nameof(Visibility.Public)); break;
            case Visibility.Unlisted: writer.WriteStringValue(nameof(Visibility.Unlisted)); break;
            case Visibility.Private: writer.WriteStringValue(nameof(Visibility.Private)); break;
            case Visibility.Direct: writer.WriteStringValue(nameof(Visibility.Direct)); break;
            default: writer.WriteStringValue(""); break;
        }
    }
}