using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
    public override FilterContext ReadJson(JsonReader reader, Type objectType, FilterContext existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        FilterContext context = 0;
        var contextStrings = serializer.Deserialize<IEnumerable<string>>(reader);
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

    public override void WriteJson(JsonWriter writer, FilterContext value, JsonSerializer serializer)
    {
        var contextStrings = new List<string>();
        if ((value & FilterContext.Home) == FilterContext.Home) contextStrings.Add("home");
        if ((value & FilterContext.Notifications) == FilterContext.Notifications) contextStrings.Add("notifications");
        if ((value & FilterContext.Public) == FilterContext.Public) contextStrings.Add("public");
        if ((value & FilterContext.Thread) == FilterContext.Thread) contextStrings.Add("thread");
        serializer.Serialize(writer, contextStrings);
    }
}
