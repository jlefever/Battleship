using Battleship.Messages;
using System.Collections.Generic;

namespace Battleship.DFA
{
    /// <summary>
    /// A INetworkState corresponds to the state of the conversation between a server and client.
    /// Most importantly, a INetworkState includes handlers where (1) we update the device state
    /// to keep in sync with the conversation state and (2) do any application behavior.
    /// </summary>
    public interface INetworkState
    {
        /// <summary>
        /// A whilelist of MessageTypeIds that can be received from this state. Nothing on this list
        /// should be passed to Receive().
        /// </summary>
        IEnumerable<MessageTypeId> ValidReceives { get; }

        /// <summary>
        /// A whilelist of MessageTypeIds that can be sent from this state. Nothing on this list
        /// should be passed to Sent().
        /// </summary>
        IEnumerable<MessageTypeId> ValidSends { get; }

        /// <summary>
        /// Called when a message has been received in this INetworkState.
        /// </summary>
        /// <param name="context">Used to change the state</param>
        /// <param name="message">The message that was received</param>
        void Received(NetworkStateContext context, IMessage message);

        /// <summary>
        /// Called when a message has been sent in this INetworkState.
        /// </summary>
        /// <param name="context">Used to change the state</param>
        /// <param name="message">The message that was sent</param>
        void Sent(NetworkStateContext context, IMessage message);
    }

    public interface INotConnected : INetworkState { }
    public interface IPendingLogOn : INetworkState { }
    public interface IWaitingForBoard : INetworkState { }
    public interface IPendingBoard : INetworkState { }
    public interface IWaitingForGame : INetworkState { }
    public interface IFoundGame : INetworkState { }
    public interface IInitialGame : INetworkState { }
    public interface IMyTurn : INetworkState { }
    public interface ITheirTurn : INetworkState { }
    public interface IWaiting : INetworkState { }
}
