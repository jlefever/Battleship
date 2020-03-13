using System;
using Battleship.Messages;

namespace Battleship.DFA
{
    public class NetworkStateContext
    {
        private readonly BspSender _sender;
        private readonly NetworkStateContainer _container;
        private INetworkState _state;

        public NetworkStateContext(BspSender sender, NetworkStateContainer container)
        {
            _sender = sender;
            _container = container;
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

        public void SetState(NetworkStateId id)
        {
            _state = _container.GetNetworkState(id);
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
