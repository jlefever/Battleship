using Battleship.Client.DFA;
using Battleship.DFA;

namespace Battleship.Client
{
    public class ClientNetworkStateContainer : NetworkStateContainer
    {
        public ClientNetworkStateContainer(Prompter prompter)
        {
            NotConnected = new NotConnected();
            PendingLogOn = new PendingLogOn(prompter);
            WaitingForBoard = new WaitingForBoard();
            PendingBoard = new PendingBoard(prompter);
            WaitingForGame = new WaitingForGame(prompter);
            FoundGame = new FoundGame(prompter);
            InitialGame = new InitialGame(prompter);
            MyTurn = new MyTurn();
            TheirTurn = new TheirTurn(prompter);
            Waiting = new Waiting(prompter);
        }

        public override INotConnected NotConnected { get; }
        public override IPendingLogOn PendingLogOn { get; }
        public override IWaitingForBoard WaitingForBoard { get; }
        public override IPendingBoard PendingBoard { get; }
        public override IWaitingForGame WaitingForGame { get; }
        public override IFoundGame FoundGame { get; }
        public override IInitialGame InitialGame { get; }
        public override IMyTurn MyTurn { get; }
        public override ITheirTurn TheirTurn { get; }
        public override IWaiting Waiting { get; }
    }
}
