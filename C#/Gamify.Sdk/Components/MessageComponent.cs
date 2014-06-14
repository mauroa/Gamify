﻿using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class MessageComponent : GameComponent
    {
        private readonly ISerializer serializer;

        public MessageComponent(INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.Message || request.Type == (int)GameRequestType.TypingMessage;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.Message || notification.Type == (int)GameNotificationType.TypingMessage;
        }

        public override void HandleRequest(GameRequest request)
        {
            if (request.Type == (int)GameRequestType.Message)
            {
                var messageObject = this.serializer.Deserialize<MessageRequestObject>(request.SerializedRequestObject);
                var notification = new MessageNotificationObject
                {
                    FromPlayerName = messageObject.PlayerName,
                    Message = messageObject.Message
                };

                this.notificationService.Send(GameNotificationType.Message, notification, messageObject.ToPlayerName);
            }
            else
            {
                var typingMessageObject = this.serializer.Deserialize<TypingMessageRequestObject>(request.SerializedRequestObject);
                var notification = new TypingMessageNotificationObject
                {
                    FromPlayerName = typingMessageObject.PlayerName,
                    Message = typingMessageObject.TypingMessage
                };

                this.notificationService.Send(GameNotificationType.TypingMessage, notification, typingMessageObject.ToPlayerName);
            }
        }
    }
}