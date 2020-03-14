using Battleship.Loggers;
using Battleship.Messages;
using System.Collections.Generic;

namespace Battleship.DFA.Server
{
    public class WaitingForGame : IWaitingForGame
    {
        private readonly BspServerState _state;
        private readonly BspSender _sender;
        private readonly ILogger _logger;
        private readonly MatchMaker _matchMaker;

        public WaitingForGame(BspServerState state, BspSender sender, ILogger logger, MatchMaker matchMaker)
        {
            _state = state;
            _sender = sender;
            _logger = logger;
            _matchMaker = matchMaker;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.RecallBoard
        };

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.FoundGame
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // The only valid receive is RecallBoard
            context.SetState(NetworkStateId.WaitingForBoard);
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // The only valid send is FoundGame
            context.SetState(NetworkStateId.FoundGame);
        }
    }
}
