using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Client.DFA
{
    public class Waiting : IWaiting
    {
        private readonly Prompter _prompter;

        public Waiting(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.YouWin,
            MessageTypeId.Hit,
            MessageTypeId.Miss,
            MessageTypeId.Sunk
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.YouWin)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                _prompter.PromptYouWin();
                _prompter.PromptWaitingForBoard();
                return;
            }

            context.SetState(NetworkStateId.TheirTurn);

            switch (message.TypeId)
            {
                case MessageTypeId.Hit:
                    _prompter.PromptYouHit();
                    break;
                case MessageTypeId.Sunk:
                    _prompter.PromptYouSunk();
                    break;
                case MessageTypeId.Miss:
                    _prompter.PromptYouMissed();
                    break;
            }

            _prompter.PromptTheirTurn();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
