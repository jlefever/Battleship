using Battleship.Loggers;
using Battleship.Repositories;

namespace Battleship.DFA
{
    public class ServerNetworkStateContainer : NetworkStateContainer
    {
        public ServerNetworkStateContainer(BspSender sender, ILogger logger, UserRepository repository)
        {
            NotConnected = new Server.NotConnected(sender, logger, repository);
            PendingLogOn = new Server.PendingLogOn(sender, logger);
            WaitingForBoard = new Server.WaitingForBoard(sender, logger);
            PendingBoard = new Server.PendingBoard(sender, logger);
            WaitingForGame = new Server.WaitingForGame(sender, logger);
            FoundGame = new Server.FoundGame(sender, logger);
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
