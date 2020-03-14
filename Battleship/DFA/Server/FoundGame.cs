using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Collections.Generic;
using Battleship.DataTypes;

namespace Battleship.DFA.Server
{
    public class FoundGame : IFoundGame
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public FoundGame(BspServerState state, BspSender sender, ILogger logger)
        {
            _state = state;
            _sender = sender;
            _logger = logger;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.AcceptGame,
            MessageTypeId.RejectGame
        };

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.RejectGame,
            MessageTypeId.GameExpired
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.RejectGame)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                return;
            }

            if (message.TypeId == MessageTypeId.AcceptGame)
            {
                _state.Match.Player.MatchResponse = MatchResponse.Accept;

                if (_state.Match.Opponent.MatchResponse == MatchResponse.Accept)
                {
                    context.SetState(NetworkStateId.InitialGame);
                }

                // do something
            }
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
