using Battleship.DFA;
using Battleship.Repositories;
using Battleship.Server.DFA;

namespace Battleship.Server
{
    public class ServerNetworkStateContainer : NetworkStateContainer
    {
        public ServerNetworkStateContainer(BspSender sender, IBspDisconnecter disconnecter, 
            GameTypeRepository gameTypeRepo, UserRepository userRepo, MatchMaker matchMaker)
        {
            var state = new BspServerState();

            NotConnected = new NotConnected(state, sender, gameTypeRepo, userRepo);
            PendingLogOn = new PendingLogOn();
            WaitingForBoard = new WaitingForBoard(state, sender, gameTypeRepo, matchMaker);
            PendingBoard = new PendingBoard();
            WaitingForGame = new WaitingForGame(state, sender, disconnecter, userRepo);
            FoundGame = new FoundGame(state, sender, disconnecter, userRepo);
            InitialGame = new InitialGame();
            MyTurn = new MyTurn(state, sender, disconnecter, userRepo);
            TheirTurn = new TheirTurn();
            Waiting = new Waiting();
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
