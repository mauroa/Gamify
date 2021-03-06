﻿using Gamify.Sdk.Interfaces;
using Gamify.Sdk.Setup.Definition;
using ThinkUp.Sdk.Services;

namespace Gamify.Sdk.Services
{
    public class MoveService<TMove, UResponse> : IMoveService<TMove, UResponse>
    {
        private readonly ISessionService sessionService;
        private readonly IMoveProcessor<TMove, UResponse> moveProcessor;

        public MoveService(ISessionService sessionService, IMoveProcessor<TMove, UResponse> moveProcessor)
        {
            this.sessionService = sessionService;
            this.moveProcessor = moveProcessor;
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public IGameMoveResponse<UResponse> Handle(string sessionName, string playerName, IGameMove<TMove> move)
        {
            var existingSession = this.sessionService.GetByName(sessionName);

            if (!existingSession.HasPlayer(playerName))
            {
                var errorMessage = string.Format("Player {0} does not belong to the session {1}", playerName, sessionName);

                throw new ServiceException(errorMessage);
            }

            var playerToCall = existingSession.GetVersusPlayer(playerName);

            return this.moveProcessor.Process(playerToCall, move);
        }
    }
}
