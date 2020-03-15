using Battleship.DFA;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.Client.DFA
{
    public class TheirTurn : ITheirTurn
    {
        private readonly Prompter _prompter;

        public TheirTurn(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new[]
        {
            MessageTypeId.TheirGuess,
            MessageTypeId.YouLose
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.YouLose)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                _prompter.PromptYouLose();
                _prompter.PromptWaitingForBoard();
                return;
            }

            context.SetState(NetworkStateId.MyTurn);
            var guess = (TheirGuessMessage)message;
            _prompter.PromptTheirGuess(guess.Position);
            _prompter.PromptMyTurn();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
