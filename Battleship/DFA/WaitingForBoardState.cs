using Battleship.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship.DFA
{
    public class WaitingForBoardState : IClientState
    {
        public IEnumerable<Type> ValidMessageTypes => throw new NotImplementedException();

        public void Received(ClientStateContext context, IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
