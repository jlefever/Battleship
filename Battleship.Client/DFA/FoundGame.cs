using Battleship.DFA;
using Battleship.Messages;
using System;
using System.Collections.Generic;

namespace Battleship.Client.DFA
{
    public class FoundGame : IFoundGame
    {
        private readonly Prompter _prompter;

        public FoundGame(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.AcceptGame,
            MessageTypeId.RejectGame,
            MessageTypeId.GameExpired
        };

        public IEnumerable<MessageTypeId> ValidSends => new[]
        {
            MessageTypeId.AcceptGame,
            MessageTypeId.RejectGame
        };

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.AcceptGame)
            {
                context.SetState(NetworkStateId.InitialGame);
                _prompter.PromptOpponentAccepted();
                return;
            }

            if (message.TypeId == MessageTypeId.RejectGame)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                _prompter.PromptOpponentRejected();
                _prompter.PromptWaitingForBoard();
                return;
            }

            context.SetState(NetworkStateId.WaitingForBoard);
            _prompter.PromptMatchExpired();
            _prompter.PromptWaitingForBoard();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
