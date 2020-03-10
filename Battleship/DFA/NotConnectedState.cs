using Battleship.Messages;
using System;

namespace Battleship.DFA
{
    public class NotConnectedState : IClientState
    {
        public void Received(ClientStateContext context, IMessage message)
        {
            // Client should not receive anything in this state.
            // Signal an illegal operation and sever the connection.
            // context.Abort(); ?
            throw new NotImplementedException();
        }
    }
}
