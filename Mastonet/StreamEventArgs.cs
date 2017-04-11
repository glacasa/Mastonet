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
        public int StatusId { get; set; }
    }
}
