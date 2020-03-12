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
            typeof(RejectLogOnMessage),
            //typeof(AcceptLogOnMessage)
        };

        public void Received(ClientStateContext context, IMessage message)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (message is RejectLogOnMessage)
            {
                context.State = new NotConnectedState();
            }

            

            context.Abort();
        }
    }
}
