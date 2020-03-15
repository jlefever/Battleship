﻿using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Client.DFA
{
    public class PendingBoard : IPendingBoard
    {
        private readonly Prompter _prompter;

        public PendingBoard(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.RejectBoard,
            MessageTypeId.AcceptBoard
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.AcceptBoard)
            {
                context.SetState(NetworkStateId.WaitingForGame);
                _prompter.PromptValidBoard();
                return;
            }

            context.SetState(NetworkStateId.WaitingForBoard);
            _prompter.PromptInvalidBoard();
            _prompter.PromptWaitingForBoard();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
