using Battleship.DFA;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.Client.DFA
{
    public class MyTurn : IMyTurn
    {
        public IEnumerable<MessageTypeId> ValidReceives => Array.Empty<MessageTypeId>();

        public IEnumerable<MessageTypeId> ValidSends => new []
        {
            MessageTypeId.MyGuess
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            context.SetState(NetworkStateId.Waiting);
        }
    }
}
