using Mastonet.Entities;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mastonet;

public abstract class TimelineStreaming
{
    protected readonly StreamingType streamingType;
    protected readonly string? param;
    protected readonly string? accessToken;

    public event EventHandler<StreamUpdateEventArgs>? OnUpdate;
    public event EventHandler<StreamNotificationEventArgs>? OnNotification;
    public event EventHandler<StreamDeleteEventArgs>? OnDelete;
    public event EventHandler<StreamFiltersChangedEventArgs>? OnFiltersChanged;
    public event EventHandler<StreamConversationEvenTargs>? OnConversation;

    protected TimelineStreaming(StreamingType type, string? param, string? accessToken)
    {
        this.streamingType = type;
        this.param = param;
        this.accessToken = accessToken;
    }

    public abstract Task Start();
    public abstract void Stop();

    protected void SendEvent(string eventName, string data)
    {
        switch (eventName)
        {
            case "update":
#if NET6_0_OR_GREATER
                var status = JsonSerializer.Deserialize(data, TryDeserializeContext.Default.Status);
#else
var status = JsonSerializer.Deserialize<Status>(data);
#endif
                if (status != null)
                {
                    OnUpdate?.Invoke(this, new StreamUpdateEventArgs(status));
                }
                break;
            case "notification":
#if NET6_0_OR_GREATER
                var notification = JsonSerializer.Deserialize(data, TryDeserializeContext.Default.Notification);
#else
var notification = JsonSerializer.Deserialize<Notification>(data);
#endif
                if (notification != null)
                {
                    OnNotification?.Invoke(this, new StreamNotificationEventArgs(notification));
                }
                break;
            case "delete":
                if (long.TryParse(data, out long statusId))
                {
                    OnDelete?.Invoke(this, new StreamDeleteEventArgs(statusId));
                }
                break;
            case "filters_changed":
                OnFiltersChanged?.Invoke(this, new StreamFiltersChangedEventArgs());
                break;
            case "conversation":
#if NET6_0_OR_GREATER
                var conversation = JsonSerializer.Deserialize(data, TryDeserializeContext.Default.Conversation);
#else
var conversation = JsonSerializer.Deserialize<Conversation>(data);
#endif
                if (conversation != null)
                {
                    OnConversation?.Invoke(this, new StreamConversationEvenTargs(conversation));
                }
                break;
        }
    }

}