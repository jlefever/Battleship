using System.Linq;
using Battleship.Messages;

namespace Battleship.DFA
{
    public class NetworkStateContext
    {
        private readonly NetworkStateContainer _container;
        private readonly IBspDisconnecter _disconnecter;
        private INetworkState _state;

        public NetworkStateContext(NetworkStateContainer container, IBspDisconnecter disconnecter)
        {
            _container = container;
            _disconnecter = disconnecter;
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
                _disconnecter.Disconnect();
                return;
            }

            _state.Received(this, message);
        }

        public void Sent(IMessage message)
        {
            if (!_state.ValidSends.Contains(message.TypeId))
            {
                _disconnecter.Disconnect();
                return;
            }

            _state.Sent(this, message);
        }
    }
}
