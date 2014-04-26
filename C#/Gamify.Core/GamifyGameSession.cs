﻿using Gamify.Sdk;
using System;

namespace Gamify.Server
{
    public class GamifyGameSession : IGameSession
    {
        public string Id { get; private set; }

        public bool IsReady { get { return this.Player1 != null && this.Player2 != null; } }

        public IGamePlayer Player1 { get; private set; }

        public IGamePlayer Player2 { get; private set; }

        public GamifyGameSession()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public GamifyGameSession(IGamePlayer player1)
            : this()
        {
            this.AddPlayer(player1);
        }

        public GamifyGameSession(IGamePlayer player1, IGamePlayer player2)
            : this(player1)
        {
            this.AddPlayer(player2);
        }

        public void AddPlayer(IGamePlayer player)
        {
            if (this.IsReady)
            {
                throw new ApplicationException("The game session is full");
            }

            if (this.Player1 == null)
            {
                this.Player1 = player;
            }
            else
            {
                this.Player2 = player;
            }
        }

        public IGamePlayer GetPlayer(string playerName)
        {
            var player = default(IGamePlayer);

            this.ValidatePlayer(playerName);

            if (this.Player1 != null && this.Player1.Name == playerName)
            {
                player = this.Player1;
            }
            else if (this.Player2 != null && this.Player2.Name == playerName)
            {
                player = this.Player2;
            }

            return player;
        }

        public void RemovePlayer(string playerName)
        {
            this.ValidatePlayer(playerName);

            if (this.Player1 != null && this.Player1.Name == playerName)
            {
                this.Player1 = null;
            }
            else if (this.Player2 != null && this.Player2.Name == playerName)
            {
                this.Player2 = null;
            }
        }

        public bool HasPlayer(string playerName)
        {
            return (this.Player1 != null && this.Player1.Name == playerName) || (this.Player2 != null && this.Player2.Name == playerName);
        }

        private void ValidatePlayer(string playerName)
        {
            if(!this.HasPlayer(playerName))
            {
                var message = string.Format("Player {0} is not part of this session", playerName);

                throw new ApplicationException(message);
            }
        }
    }
}
