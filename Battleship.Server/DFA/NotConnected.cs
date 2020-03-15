using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Server.DFA
{
    public class NotConnected : INotConnected
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly GameTypeRepository _gameTypeRepo;
        private readonly UserRepository _userRepo;

        public NotConnected(BspServerState state, BspSender sender, 
            GameTypeRepository gameTypeRepo, UserRepository userRepo)
        {
            _state = state;
            _sender = sender;
            _gameTypeRepo = gameTypeRepo;
            _userRepo = userRepo;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.LogOn
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            // Change state to PendingLogOn so if the client sends any messages
            // in the meantime, that state will take over.
            context.SetState(NetworkStateId.PendingLogOn);

            var attempt = (LogOnMessage)message;

            if (attempt.Version != BspConstants.Version)
            {
                _sender.Send(new RejectLogOnMessage(BspConstants.Version));
                return;
            }

            if (!_userRepo.TryLogIn(attempt.Username, attempt.Password, _sender))
            {
                _sender.Send(new RejectLogOnMessage(BspConstants.Version));
                return;
            }

            // Client has successfully logged in, so we update our record.
            _state.Username = attempt.Username;

            // Let the user know they were successful.
            _sender.Send(new BasicMessage(MessageTypeId.AcceptLogOn));

            // Send them the game types.
            foreach (var gameType in _gameTypeRepo.GetAll())
            {
                _sender.Send(new GameTypeMessage(gameType));
            }
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // There are no valid messages for the server to send in this state.
        }
    }
}
