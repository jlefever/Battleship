using Battleship.Messages;

namespace Battleship.DFA
{
    public class NotConnectedState : INetworkState
    {
        public void ServerReceived(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId != MessageTypeId.LogOn)
            {
                context.Disconnect();
            }

            context.SetState(new PendingLogOnState());
            context.ServerReceived(message);
        }

        public void ClientReceived(NetworkStateContext context, IMessage message)
        {
            context.Disconnect();
        }
    }
}
