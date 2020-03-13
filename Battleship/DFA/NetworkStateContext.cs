using System;
using Battleship.Messages;

namespace Battleship.DFA
{
    public class NetworkStateContext
    {
        private readonly BspSender _sender;
        private INetworkState _state;

        public NetworkStateContext(BspSender sender)
        {
            _sender = sender;
            _state = new NotConnectedState();
        }

        public void Send(IMessage message)
        {
            _ = _sender.SendAsync(message);
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SetState(INetworkState state)
        {
            _state = state;
        }

        public void ServerReceived(IMessage message)
        {
            _state.ServerReceived(this, message);
        }

        public void ClientReceived(IMessage message)
        {
            _state.ClientReceived(this, message);
        }
    }
}
