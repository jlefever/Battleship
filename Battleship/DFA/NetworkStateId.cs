namespace Battleship.DFA
{
    public enum NetworkStateId
    {
        NotConnected,
        PendingLogOn,
        WaitingForBoard,
        PendingBoard,
        WaitingForGame,
        FoundGame,
        InitialGame,
        MyTurn,
        TheirTurn,
        Waiting
    }
}
