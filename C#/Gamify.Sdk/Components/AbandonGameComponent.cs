﻿using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class AbandonGameComponent : GameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISerializer serializer;

        public AbandonGameComponent(ISessionService sessionService, INotificationService notificationService, 
            ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.AbandonGame;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.GameAbandoned;
        }

        public override void HandleRequest(GameRequest request)
        {
            var abandonGameObject = this.serializer.Deserialize<AbandonGameRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(abandonGameObject.SessionName);

            this.sessionService.Abandon(currentSession.Name);

            this.SendAbandonGameNotification(abandonGameObject, currentSession);
        }

        private void SendAbandonGameNotification(AbandonGameRequestObject abandonGameObject, IGameSession currentSession)
        {
            var notification = new GameAbandonedNotificationObject
            {
                SessionName = abandonGameObject.SessionName,
                PlayerName = abandonGameObject.PlayerName
            };

            this.notificationService.SendBroadcast(GameNotificationType.GameAbandoned, notification, currentSession.Player1Name, currentSession.Player2Name);
        }
    }
}