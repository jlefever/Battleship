using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.DFA.Server
{
    public class TheirTurn : ITheirTurn
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public TheirTurn(BspSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new []
        {
            MessageTypeId.TheirGuess,
            MessageTypeId.YouLose
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.TheirGuess)
            {
                context.SetState(NetworkStateId.MyTurn);
                return;
            }

            context.SetState(NetworkStateId.WaitingForBoard);
        }
    }
}
