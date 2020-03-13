using Battleship.Messages;
using Battleship.Repositories;

namespace Battleship.DFA
{
    public class NotConnectedState : INetworkState
    {
        private readonly UserRepository _repository;

        public NotConnectedState(UserRepository repository)
        {
            _repository = repository;
        }

        public void ServerReceived(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId != MessageTypeId.LogOn)
            {
                context.Disconnect();
            }

            // Change state to PendingLogOn so if the client sends any messages
            // in the meantime, that state will take over.
            context.SetState(NetworkStateId.PendingLogOn);

            var attempt = (LogOnMessage)message;

            if (_repository.IsValidUser(attempt.Username, attempt.Password))
            {
                context.Send(new BasicMessage(MessageTypeId.AcceptLogOn));
                context.SetState(NetworkStateId.WaitingForBoard);
                return;
            }

            context.Send(new RejectLogOnMessage(BspConstants.Version));
            context.SetState(NetworkStateId.NotConnected);
        }

        public void ClientReceived(NetworkStateContext context, IMessage message)
        {
            context.Disconnect();
        }
    }
}
