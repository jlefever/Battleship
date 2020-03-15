using Battleship.DFA;

namespace Battleship.Client
{
    public class ClientNetworkStateContainer : NetworkStateContainer
    {
        //public ClientNetworkStateContainer(BspSender sender, ILogger logger)
        //{
        //    NotConnected = new Client.NotConnected(sender, logger);
        //    PendingLogOn = new Client.PendingLogOn(sender, logger);
        //    WaitingForBoard = new Client.WaitingForBoard(sender, logger);
        //    PendingBoard = new Client.PendingBoard(sender, logger);
        //    WaitingForGame = new Client.WaitingForGame(sender, logger);
        //    FoundGame = new Client.FoundGame(sender, logger);
        //    InitialGame = new Client.InitialGame(sender, logger);
        //    MyTurn = new Client.MyTurn(sender, logger);
        //    TheirTurn = new Client.TheirTurn(sender, logger);
        //    Waiting = new Client.Waiting(sender, logger);
        //}

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
