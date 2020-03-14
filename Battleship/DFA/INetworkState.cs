using System;
using System.Collections.Generic;
using Battleship.Messages;

namespace Battleship.DFA
{
    public interface INetworkState
    {
        IEnumerable<MessageTypeId> ValidReceives { get; }
        IEnumerable<MessageTypeId> ValidSends { get; }
        void Received(NetworkStateContext context, IMessage message);
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
