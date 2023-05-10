using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Mastonet;

[Flags]
public enum NotificationType
{
    None = 0,
    Follow = 1,
    Favourite = 2,
    Reblog = 4,
    Mention = 8,
    Poll = 16,
    FollowRequest = 32,
}

public class NotificationTypeConverter : JsonConverter<NotificationType>
{
    public override NotificationType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        NotificationType context = 0;
        var contextStrings = JsonSerializer.Deserialize<IEnumerable<string>>(ref reader, options);
        if (contextStrings != null)
        {
            foreach (var contextString in contextStrings)
            {
                switch (contextString)
                {
                    case "follow":
                        context |= NotificationType.Follow;
                        break;
                    case "favourite":
                        context |= NotificationType.Favourite;
                        break;
                    case "reblog":
                        context |= NotificationType.Reblog;
                        break;
                    case "mention":
                        context |= NotificationType.Mention;
                        break;
                    case "poll":
                        context |= NotificationType.Poll;
                        break;
                }
            }
        }

        return context;
    }

    public override void Write(Utf8JsonWriter writer, NotificationType value, JsonSerializerOptions options)
    {
        var contextStrings = new List<string>();
        if ((value & NotificationType.Follow) == NotificationType.Follow) contextStrings.Add("follow");
        if ((value & NotificationType.Favourite) == NotificationType.Favourite) contextStrings.Add("favourite");
        if ((value & NotificationType.Reblog) == NotificationType.Reblog) contextStrings.Add("reblog");
        if ((value & NotificationType.Mention) == NotificationType.Mention) contextStrings.Add("mention");
        if ((value & NotificationType.Poll) == NotificationType.Poll) contextStrings.Add("poll");
        JsonSerializer.Serialize(writer, contextStrings, options);
    }
}