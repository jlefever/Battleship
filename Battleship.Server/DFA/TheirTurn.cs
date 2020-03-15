using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Server.DFA
{
    public class TheirTurn : ITheirTurn
    {
        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new[]
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
