using Battleship.Loggers;
using Battleship.Messages;
using Battleship.Repositories;

namespace Battleship.DFA.Server
{
    public class NotConnected : INotConnected
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;
        private readonly UserRepository _repository;

        public NotConnected(BspSender sender, ILogger logger, UserRepository repository)
        {
            _sender = sender;
            _logger = logger;
            _repository = repository;
        }

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId != MessageTypeId.LogOn)
            {
                _sender.Disconnect();
            }

            // Change state to PendingLogOn so if the client sends any messages
            // in the meantime, that state will take over.
            context.SetState(NetworkStateId.PendingLogOn);

            var attempt = (LogOnMessage)message;

            if (_repository.IsValidUser(attempt.Username, attempt.Password))
            {
                _ = _sender.SendAsync(new BasicMessage(MessageTypeId.AcceptLogOn));
                context.SetState(NetworkStateId.WaitingForBoard);
                return;
            }

            _ = _sender.SendAsync(new RejectLogOnMessage(BspConstants.Version));
            context.SetState(NetworkStateId.NotConnected);
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is intentionally left blank.
        }
    }
}
