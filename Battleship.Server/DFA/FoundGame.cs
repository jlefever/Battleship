using System.Collections.Generic;
using Battleship.DataTypes;
using Battleship.DFA;
using Battleship.Messages;
using Battleship.Repositories;

namespace Battleship.Server.DFA
{
    public class FoundGame : IFoundGame
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly UserRepository _userRepo;

        public FoundGame(BspServerState state, BspSender sender, UserRepository userRepo)
        {
            _state = state;
            _sender = sender;
            _userRepo = userRepo;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.AcceptGame,
            MessageTypeId.RejectGame
        };

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.AcceptGame,
            MessageTypeId.RejectGame,
            MessageTypeId.GameExpired
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // Get the opponents connection.
            if (!_userRepo.TryGetSender(_state.Match.Opponent.Username, out var opponent))
            {
                // Somehow the opponent is not logged in.
                _sender.Disconnect();
                return;
            }

            _state.CancelMatchTimer();

            // The player is rejecting the game.
            if (message.TypeId == MessageTypeId.RejectGame)
            {
                // Mark the player as accepting the game.
                _state.Match.Player.AcceptedGame = MatchResponse.Reject;

                // Update our state.
                context.SetState(NetworkStateId.WaitingForBoard);

                // Let the opponent know that the player has rejected the game.
                opponent.Send(new BasicMessage(MessageTypeId.RejectGame));
                return;
            }

            // The only remaining valid message is AcceptGame. So mark the player as accept.
            _state.Match.Player.AcceptedGame = MatchResponse.Accept;

            // If the opponent has also sent AcceptGame, we let both know to start the game.
            if (_state.Match.Opponent.AcceptedGame == MatchResponse.Accept)
            {
                // Let both know that the game has been accepted.
                _sender.Send(new BasicMessage(MessageTypeId.AcceptGame));
                opponent.Send(new BasicMessage(MessageTypeId.AcceptGame));

                // Notify who goes first
                if (_state.Match.PlayerGoesFirst)
                {
                    _sender.Send(new BasicMessage(MessageTypeId.AssignRed));
                    opponent.Send(new BasicMessage(MessageTypeId.AssignBlue));
                }
                else
                {
                    _sender.Send(new BasicMessage(MessageTypeId.AssignBlue));
                    opponent.Send(new BasicMessage(MessageTypeId.AssignRed));
                }
            }
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            switch (message.TypeId)
            {
                case MessageTypeId.AcceptGame:
                    context.SetState(NetworkStateId.InitialGame);
                    return;
                case MessageTypeId.RejectGame:
                    context.SetState(NetworkStateId.WaitingForBoard);
                    return;
                case MessageTypeId.GameExpired:
                    context.SetState(NetworkStateId.WaitingForBoard);
                    return;
            }
        }
    }
}
