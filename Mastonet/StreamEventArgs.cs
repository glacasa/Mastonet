using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet
{
    public class StreamUpdateEventArgs : EventArgs
    {
        public Status Status { get; set; } = new Status();
    }

    public class StreamNotificationEventArgs : EventArgs
    {
        public Notification Notification { get; set; } = new Notification();
    }

    public class StreamDeleteEventArgs : EventArgs
    {
        public long StatusId { get; set; }
    }
}
