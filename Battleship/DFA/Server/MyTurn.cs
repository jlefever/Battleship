using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Collections.Generic;
using Battleship.DataTypes;
using Battleship.Repositories;


namespace Battleship.DFA.Server
{
    public class MyTurn : IMyTurn
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly ILogger _logger;
        private readonly UserRepository _userRepo;

        public MyTurn(BspServerState state, BspSender sender, ILogger logger, UserRepository userRepo)
        {
            _state = state;
            _sender = sender;
            _logger = logger;
            _userRepo = userRepo;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.MyGuess
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            // Only valid receive is MyGuess
            context.SetState(NetworkStateId.Waiting);

            var guess = ((MyGuessMessage) message).Position;
            var guessResult = _state.Match.Opponent.Board.Guess(guess);

            var id = guessResult switch
            {
                GuessResult.Miss => MessageTypeId.Miss,
                GuessResult.Hit => MessageTypeId.Hit,
                GuessResult.Sunk => MessageTypeId.Sunk,
                GuessResult.Win => MessageTypeId.YouWin,
                _ => throw new ArgumentOutOfRangeException()
            };

            _sender.Send(new BasicMessage(id));

            // Get the opponents connection.
            if (!_userRepo.TryGetSender(_state.Match.Opponent.Username, out var opponent))
            {
                // Somehow the opponent is not logged in.
                _sender.Disconnect();
                return;
            }

            if (id == MessageTypeId.YouWin)
            {
                opponent.Send(new YouLoseMessage(guess));
                return;
            }

            opponent.Send(new TheirGuessMessage(guess));
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
