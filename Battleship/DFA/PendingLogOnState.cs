using Battleship.Messages;

namespace Battleship.DFA
{
    public class PendingLogOnState : INetworkState
    {
        public void ServerReceived(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId != MessageTypeId.LogOn)
            {
                context.Disconnect();
            }

            var attempt = (LogOnMessage)message;

            if (attempt.Username == "jason")
            {
                context.Send(new BasicMessage(MessageTypeId.AcceptLogOn));
                context.SetState(NetworkStateId.WaitingForBoard);
                return;
            }

            context.Send(new RejectLogOnMessage(0));
            context.SetState(NetworkStateId.NotConnected);
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
