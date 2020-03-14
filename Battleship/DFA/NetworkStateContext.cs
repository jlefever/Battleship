using Battleship.Messages;

namespace Battleship.DFA
{
    public class NetworkStateContext
    {
        private readonly NetworkStateContainer _container;
        private INetworkState _state;

        public NetworkStateContext(NetworkStateContainer container)
        {
            _container = container;
            _state = container.GetNetworkState(NetworkStateId.NotConnected);
        }

        public void SetState(NetworkStateId id)
        {
            _state = _container.GetNetworkState(id);
        }

        public void Received(IMessage message)
        {
            _state.Received(this, message);
        }

        public void Sent(IMessage message)
        {
            _state.Sent(this, message);
        }
    }
}
