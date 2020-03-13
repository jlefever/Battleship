using Battleship.Messages;

namespace Battleship.DFA
{
    public class PendingLogOnState : INetworkState
    {
        public void ServerReceived(NetworkStateContext context, IMessage message)
        {
            context.Disconnect();
        }

        public void ClientReceived(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.RejectLogOn)
            {
                context.SetState(NetworkStateId.NotConnected);
                return;
            }

            if (message.TypeId == MessageTypeId.AcceptLogOn)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                return;
            }

            context.Disconnect();
        }
    }
}
