using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mastonet;

public class JsonEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (str != null && TryParse(str, out T result))
        {
            return result;
        }

        return default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var snakeCase = ToSnakeCase(value);
        writer.WriteStringValue(snakeCase);
    }

    public static bool TryParse(string str, out T result)
    {
        var val = str.Replace("_", "");
        return Enum.TryParse(val, true, out result);
    }

    public static string ToSnakeCase(T value)
    {
        return string.Concat(value.ToString().Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }
}
