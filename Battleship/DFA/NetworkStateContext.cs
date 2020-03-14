using System.Linq;
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
            _state = container.GetNetworkState(NetworkStateId.NotConnected);
        }

        public void SetState(NetworkStateId id)
        {
            _state = _container.GetNetworkState(id);
        }

        public void Received(IMessage message)
        {
            if (!_state.ValidReceives.Contains(message.TypeId))
            {
                _sender.Disconnect();
                return;
            }

            _state.Received(this, message);
        }

        public void Sent(IMessage message)
        {
            if (!_state.ValidSends.Contains(message.TypeId))
            {
                _sender.Disconnect();
                return;
            }

            _state.Sent(this, message);
        }
    }
}
