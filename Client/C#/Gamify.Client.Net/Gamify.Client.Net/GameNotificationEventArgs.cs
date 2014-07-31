﻿using System;

namespace Gamify.Client.Net
{
    public class GameNotificationEventArgs<TNotification> : EventArgs
    {
        public TNotification NotificationObject { get; private set; }

        public GameNotificationEventArgs(TNotification notificationObject)
        {
            this.NotificationObject = notificationObject;
        }
    }
}