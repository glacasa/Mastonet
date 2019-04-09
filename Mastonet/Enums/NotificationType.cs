using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mastonet
{
    [Flags]
    public enum NotificationType
    {
        Follow = 1,
        Favourite = 2,
        Reblog = 4,
        Mention = 8
    }

    public class NotificationTypeConverter : JsonConverter<NotificationType>
    {
        public override NotificationType ReadJson(JsonReader reader, Type objectType, NotificationType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            NotificationType context = 0;
            var contextStrings = serializer.Deserialize<IEnumerable<string>>(reader);
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
            serializer.Serialize(writer, contextStrings);
        }
    }
}