using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet
{
    public class StreamUpdateEventArgs : EventArgs
    {
        public Status Status { get; set; }
    }

    public class StreamNotificationEventArgs : EventArgs
    {
        public Notification Notification { get; set; }
    }

    public class StreamDeleteEventArgs : EventArgs
    {
        public long StatusId { get; set; }
    }

    public class StreamFiltersChangedEventArgs : EventArgs
    {
    }

    public class StreamConversationEvenTargs : EventArgs
    {
        public Conversation Conversation { get; set; }
    }
}
