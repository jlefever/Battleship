using Battleship.Messages;

namespace Battleship.DFA
{
    public interface INetworkState
    {
        void ServerReceived(NetworkStateContext context, IMessage message);
        void ClientReceived(NetworkStateContext context, IMessage message);
    }
}
