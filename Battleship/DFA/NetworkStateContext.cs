using System.Linq;
using Battleship.Messages;

namespace Battleship.DFA
{
    /// <summary>
    /// Contains the current state of the conversation between a client and server and
    /// allows that state to be updated with SetState().
    /// </summary>
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

        /// <summary>
        /// Update the state of the conversation.
        /// </summary>
        /// <param name="id"></param>
        public void SetState(NetworkStateId id)
        {
            _state = _container.GetNetworkState(id);
        }

        /// <summary>
        /// Call with a received message. If this message is not the current state's
        /// ValidReceives list, the connection will be disconnected.
        /// </summary>
        /// <param name="message">A received message</param>
        public void Received(IMessage message)
        {
            if (!_state.ValidReceives.Contains(message.TypeId))
            {
                _disconnecter.Disconnect();
                return;
            }

            _state.Received(this, message);
        }

        /// <summary>
        /// Call with a sent message. If this message is not the current state's
        /// ValidSends list, the connection will be disconnected.
        /// </summary>
        /// <param name="message">A sent message</param>
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
