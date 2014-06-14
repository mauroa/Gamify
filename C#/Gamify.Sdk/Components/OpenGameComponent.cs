﻿using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Components
{
    public class OpenGameComponent<TMove, UResponse> : GameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService<TMove, UResponse> sessionHistoryService;
        private readonly IGameInformationNotificationFactory<TMove, UResponse> gameInformationNotificationFactory;
        private readonly IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory;
        private readonly ISerializer serializer;

        public OpenGameComponent(ISessionService sessionService, ISessionHistoryService<TMove, UResponse> sessionHistoryService, 
            INotificationService notificationService, IGameInformationNotificationFactory<TMove, UResponse> gameInformationNotificationFactory, 
            IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory, ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.gameInformationNotificationFactory = gameInformationNotificationFactory;
            this.playerHistoryItemFactory = playerHistoryItemFactory;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.OpenGame;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.SendGameInformation;
        }

        public override void HandleRequest(GameRequest request)
        {
            var openGameObject = this.serializer.Deserialize<OpenGameRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(openGameObject.SessionName);
            var gameInformationNotificationObject = this.gameInformationNotificationFactory.Create(currentSession, this.sessionHistoryService, this.playerHistoryItemFactory);

            this.notificationService.Send(GameNotificationType.SendGameInformation, gameInformationNotificationObject, openGameObject.PlayerName);
        }
    }
}