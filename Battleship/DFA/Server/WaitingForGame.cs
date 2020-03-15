﻿using System;
using Battleship.Loggers;
using Battleship.Messages;
using System.Collections.Generic;
using System.Timers;
using Battleship.DataTypes;
using Battleship.Repositories;

namespace Battleship.DFA.Server
{
    public class WaitingForGame : IWaitingForGame
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly ILogger _logger;
        private readonly MatchMaker _matchMaker;
        private readonly UserRepository _userRepo;

        public WaitingForGame(BspServerState state, BspSender sender, ILogger logger,
            MatchMaker matchMaker, UserRepository userRepo)
        {
            _state = state;
            _sender = sender;
            _logger = logger;
            _matchMaker = matchMaker;
            _userRepo = userRepo;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.RecallBoard
        };

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.FoundGame
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // The only valid receive is RecallBoard
            context.SetState(NetworkStateId.WaitingForBoard);
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // The only valid send is FoundGame
            context.SetState(NetworkStateId.FoundGame);

            // Get the opponents connection.
            if (!_userRepo.TryGetSender(_state.Match.Opponent.Username, out var opponent))
            {
                // Somehow the opponent is not logged in.
                _sender.Disconnect();
                return;
            }

            _state.SetMatchTimeoutCallback((s, e) =>
            {
                // If the opponent has responded already, send a RejectGame to them.
                if (_state.Match.Opponent.AcceptedGame != MatchResponse.None)
                {
                    opponent.Send(new BasicMessage(MessageTypeId.RejectGame));
                }

                _sender.Send(new BasicMessage(MessageTypeId.GameExpired));
            });

            _state.StartMatchTimer();
        }
    }
}
