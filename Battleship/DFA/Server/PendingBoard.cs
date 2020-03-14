using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.DFA.Server
{
    public class PendingBoard : IPendingBoard
    {
        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.AcceptBoard,
            MessageTypeId.RejectBoard
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            switch (message.TypeId)
            {
                case MessageTypeId.AcceptBoard:
                    context.SetState(NetworkStateId.WaitingForGame);
                    return;
                case MessageTypeId.RejectBoard:
                    context.SetState(NetworkStateId.WaitingForBoard);
                    return;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
