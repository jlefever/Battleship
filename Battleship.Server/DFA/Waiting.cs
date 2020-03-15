using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Server.DFA
{
    public class Waiting : IWaiting
    {
        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new[]
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
