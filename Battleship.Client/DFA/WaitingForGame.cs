using Battleship.DFA;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.Client.DFA
{
    public class WaitingForGame : IWaitingForGame
    {
        private readonly Prompter _prompter;

        public WaitingForGame(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.FoundGame
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            context.SetState(NetworkStateId.FoundGame);
            _prompter.PromptFoundGame();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
