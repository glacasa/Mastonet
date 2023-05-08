using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
    public override NotificationType ReadJson(JsonReader reader, Type objectType, NotificationType existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        NotificationType context = NotificationType.None;
        var contextStrings = serializer.Deserialize<IEnumerable<string>>(reader);
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

    public override void WriteJson(JsonWriter writer, NotificationType value, JsonSerializer serializer)
    {
        var contextStrings = new List<string>();
        if ((value & NotificationType.Follow) == NotificationType.Follow) contextStrings.Add("follow");
        if ((value & NotificationType.Favourite) == NotificationType.Favourite) contextStrings.Add("favourite");
        if ((value & NotificationType.Reblog) == NotificationType.Reblog) contextStrings.Add("reblog");
        if ((value & NotificationType.Mention) == NotificationType.Mention) contextStrings.Add("mention");
        if ((value & NotificationType.Poll) == NotificationType.Poll) contextStrings.Add("poll");
        serializer.Serialize(writer, contextStrings);
    }
}