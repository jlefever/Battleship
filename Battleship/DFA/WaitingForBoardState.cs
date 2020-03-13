using Battleship.Messages;
using System;

namespace Battleship.DFA
{
    public class WaitingForBoardState : INetworkState
    {
        public void ServerReceived(NetworkStateContext context, IMessage message)
        {
            throw new NotImplementedException();
        }

        public void ClientReceived(NetworkStateContext context, IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
