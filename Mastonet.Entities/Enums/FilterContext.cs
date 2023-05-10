using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Mastonet;

[Flags]
[JsonConverter(typeof(FilterContextConverter))]
public enum FilterContext
{
    Home = 1,
    Notifications = 2,
    Public = 4,
    Thread = 8
}

public class FilterContextConverter : JsonConverter<FilterContext>
{
    public override FilterContext Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        FilterContext context = 0;
        var contextStrings = JsonSerializer.Deserialize<IEnumerable<string>>(ref reader, options);
        if (contextStrings != null) {
            foreach (var contextString in contextStrings)
            {
                switch (contextString)
                {
                    case "home":
                        context |= FilterContext.Home;
                        break;
                    case "notifications":
                        context |= FilterContext.Notifications;
                        break;
                    case "public":
                        context |= FilterContext.Public;
                        break;
                    case "thread":
                        context |= FilterContext.Thread;
                        break;
                }
            }
        }

        return context;
    }
    
    public override void Write(Utf8JsonWriter writer, FilterContext value, JsonSerializerOptions options)
    {
        var contextStrings = new List<string>();
        if ((value & FilterContext.Home) == FilterContext.Home) contextStrings.Add("home");
        if ((value & FilterContext.Notifications) == FilterContext.Notifications) contextStrings.Add("notifications");
        if ((value & FilterContext.Public) == FilterContext.Public) contextStrings.Add("public");
        if ((value & FilterContext.Thread) == FilterContext.Thread) contextStrings.Add("thread");
        JsonSerializer.Serialize(writer, contextStrings, options);
    }
    
}
