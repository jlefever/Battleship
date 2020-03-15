using System;

namespace Battleship.DFA
{
    /// <summary>
    /// This contains instances for every INetworkState required by the state machine.
    /// Principally used by NetworkStateContext to keep track of state instances.
    /// </summary>
    public abstract class NetworkStateContainer
    {
        public abstract INotConnected NotConnected { get; }
        public abstract IPendingLogOn PendingLogOn { get; }
        public abstract IWaitingForBoard WaitingForBoard { get; }
        public abstract IPendingBoard PendingBoard { get; }
        public abstract IWaitingForGame WaitingForGame { get; }
        public abstract IFoundGame FoundGame { get; }
        public abstract IInitialGame InitialGame { get; }
        public abstract IMyTurn MyTurn { get; }
        public abstract ITheirTurn TheirTurn { get; }
        public abstract IWaiting Waiting { get; }

        public INetworkState GetNetworkState(NetworkStateId id)
        {
            return id switch
            {
                NetworkStateId.NotConnected => NotConnected,
                NetworkStateId.PendingLogOn => PendingLogOn,
                NetworkStateId.WaitingForBoard => WaitingForBoard,
                NetworkStateId.PendingBoard => PendingBoard,
                NetworkStateId.WaitingForGame => WaitingForGame,
                NetworkStateId.FoundGame => FoundGame,
                NetworkStateId.InitialGame => InitialGame,
                NetworkStateId.MyTurn => MyTurn,
                NetworkStateId.TheirTurn => TheirTurn,
                NetworkStateId.Waiting => Waiting,
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
            };
        }
    }
}
