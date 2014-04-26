﻿namespace Gamify.Server.Contracts.Notifications
{
    public class UserDisconnectedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("Player {0} has been disconnected", this.PlayerName);
            }
        }

        public string PlayerName { get; set; }
    }
}
