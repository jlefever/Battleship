using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Server.DFA
{
    public class PendingLogOn : IPendingLogOn
    {
        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.AcceptLogOn,
            MessageTypeId.RejectLogOn
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // There are no valid messages for the server to receive in this state.
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.AcceptLogOn)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
            }
            else
            {
                context.SetState(NetworkStateId.NotConnected);
            }
        }
    }
}
