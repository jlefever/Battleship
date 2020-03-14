using Battleship.Loggers;
using Battleship.Messages;
using System;

namespace Battleship.DFA.Client
{
    public class PendingBoard : IPendingBoard
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public PendingBoard(BspSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public void Received(NetworkStateContext context, IMessage message)
        {
            throw new NotImplementedException();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
