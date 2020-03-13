using System;
using Battleship.Messages;

namespace Battleship.DFA
{
    public class InitialGameState : INetworkState
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