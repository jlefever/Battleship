using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Client.DFA
{
    public class InitialGame : IInitialGame
    {
        private readonly Prompter _prompter;

        public InitialGame(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.AssignRed,
            MessageTypeId.AssignBlue
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.AssignRed)
            {
                context.SetState(NetworkStateId.MyTurn);
                _prompter.PromptAssignRed();
                _prompter.PromptMyTurn();
                return;
            }

            context.SetState(NetworkStateId.TheirTurn);
            _prompter.PromptAssignBlue();
            _prompter.PromptTheirTurn();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
