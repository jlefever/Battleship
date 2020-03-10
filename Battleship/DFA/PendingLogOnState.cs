using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.DFA
{
    public class PendingLogOnState : IClientState
    {
        // I don't know where to use these yet.
        public IEnumerable<Type> ValidMessageTypes { get; } = new[] 
        { 
            typeof(RejectLogOn),
            typeof(AcceptLogOn)
        };

        public void Received(ClientStateContext context, IMessage message)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (message is RejectLogOn)
            {
                context.State = new NotConnectedState();
            }

            if (message is AcceptLogOn)
            {
                context.State = new WaitingForBoardState();
            }

            context.Abort();
        }
    }
}
