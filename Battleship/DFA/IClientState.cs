using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.DFA
{
    public interface IClientState
    {
        // IEnumerable<Type> ValidMessageTypes { get; }
        void Received(ClientStateContext context, IMessage message);
    }
}
