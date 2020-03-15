using System;
using System.Collections.Generic;
using Battleship.DFA;
using Battleship.Messages;

namespace Battleship.Client.DFA
{
    public class PendingLogOn : IPendingLogOn
    {
        private readonly Prompter _prompter;

        public PendingLogOn(Prompter prompter)
        {
            _prompter = prompter;
        }

        public IEnumerable<MessageTypeId> ValidReceives => new []
        {
            MessageTypeId.RejectLogOn,
            MessageTypeId.AcceptLogOn
        };

        public IEnumerable<MessageTypeId> ValidSends => Array.Empty<MessageTypeId>();

        public void Received(NetworkStateContext context, IMessage message)
        {
            if (message.TypeId == MessageTypeId.AcceptLogOn)
            {
                context.SetState(NetworkStateId.WaitingForBoard);
                _prompter.PromptSuccessfulLogOn();
                _prompter.PromptWaitingForBoard();
                return;
            }

            context.SetState(NetworkStateId.NotConnected);
            var version = ((RejectLogOnMessage)message).Version;
            _prompter.PromptFailedLogOn(version);
            _prompter.PromptLogOn();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // This is left intentionally blank.
        }
    }
}
