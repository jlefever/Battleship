using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.DFA.Server
{
    public class InitialGame : IInitialGame
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public InitialGame(BspSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            throw new NotImplementedException();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
