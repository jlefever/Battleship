using Battleship.Loggers;
using Battleship.Repositories;

namespace Battleship.DFA
{
    public class ServerNetworkStateContainer : NetworkStateContainer
    {
        public ServerNetworkStateContainer(BspServerState state, BspSender sender,
            ILogger logger, GameTypeRepository gameTypeRepo, UserRepository userRepo,
            MatchMaker matchMaker)
        {
            NotConnected = new Server.NotConnected(state, sender, gameTypeRepo, userRepo);
            PendingLogOn = new Server.PendingLogOn();
            WaitingForBoard = new Server.WaitingForBoard(state, sender, logger, gameTypeRepo, matchMaker);
            PendingBoard = new Server.PendingBoard();
            WaitingForGame = new Server.WaitingForGame(state, sender, logger, matchMaker, userRepo);
            FoundGame = new Server.FoundGame(state, sender, logger, userRepo);
            InitialGame = new Server.InitialGame(sender, logger);
            MyTurn = new Server.MyTurn(sender, logger);
            TheirTurn = new Server.TheirTurn(sender, logger);
            Waiting = new Server.Waiting(sender, logger);
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
