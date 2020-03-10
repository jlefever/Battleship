using Battleship.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship.DFA
{
    // Generalize to StateContext by using strategy pattern
    public class ClientStateContext
    {
        public IClientState State { get; set; }

        public ClientStateContext()
        {
            State = new NotConnectedState();
        }

        // Is this an event handler? Read more about
        // C# events.
        public void Received(IMessage message)
        {
            State.Received(this, message);
        }

        public void Send(IMessage message)
        {
            // Delegate this to another class
            throw new NotImplementedException();
        }

        public void Abort()
        {
            // Delegate this to another class
            throw new NotImplementedException();
        }
    }
}
