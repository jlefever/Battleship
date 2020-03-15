using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.DFA.Server
{
    public class Waiting : IWaiting
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public Waiting(BspSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new []
        {
            MessageTypeId.Hit,
            MessageTypeId.Miss,
            MessageTypeId.Sunk,
            MessageTypeId.YouWin
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.YouWin)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                return;
            }

            context.SetState(NetworkStateId.TheirTurn);
        }
    }
}
