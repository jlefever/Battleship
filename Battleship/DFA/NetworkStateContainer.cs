using System;
using Battleship.Repositories;

namespace Battleship.DFA
{
    public class NetworkStateContainer
    {
        private readonly NotConnectedState _notConnected;
        private readonly PendingLogOnState _pendingLogOn;
        private readonly WaitingForBoardState _waitingForBoard;
        private readonly PendingBoardState _pendingBoard;
        private readonly WaitingForGameState _waitingForGame;
        private readonly FoundGameState _foundGame;
        private readonly InitialGameState _initialGame;
        private readonly MyTurnState _myTurn;
        private readonly TheirTurnState _theirTurn;
        private readonly WaitingState _waiting;

        public NetworkStateContainer(UserRepository repository)
        {
            _notConnected = new NotConnectedState(repository);
            _pendingLogOn = new PendingLogOnState();
            _waitingForBoard = new WaitingForBoardState();
            _pendingBoard = new PendingBoardState();
            _waitingForGame = new WaitingForGameState();
            _foundGame = new FoundGameState();
            _initialGame = new InitialGameState();
            _myTurn = new MyTurnState();
            _theirTurn = new TheirTurnState();
            _waiting = new WaitingState();
        }

        public INetworkState GetNetworkState(NetworkStateId id)
        {
            return id switch
            {
                NetworkStateId.NotConnected => _notConnected,
                NetworkStateId.PendingLogOn => _pendingLogOn,
                NetworkStateId.WaitingForBoard => _waitingForBoard,
                NetworkStateId.PendingBoard => _pendingBoard,
                NetworkStateId.WaitingForGame => _waitingForGame,
                NetworkStateId.FoundGame => _foundGame,
                NetworkStateId.InitialGame => _initialGame,
                NetworkStateId.MyTurn => _myTurn,
                NetworkStateId.TheirTurn => _theirTurn,
                NetworkStateId.Waiting => _waiting,
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
            };
        }
    }
}
