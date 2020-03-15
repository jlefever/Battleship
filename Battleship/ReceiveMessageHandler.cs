using Battleship.DFA;
using Battleship.Messages;

namespace Battleship
{
    /// <summary>
    /// A message handler that passes its messages to a state machine context.
    /// </summary>
    public class ReceiveMessageHandler : IMessageHandler
    {
        private readonly NetworkStateContext _context;

        public ReceiveMessageHandler(NetworkStateContext context)
        {
            _context = context;
        }

        public void Handle(IMessage message)
        {
            _context.Received(message);
        }
    }
}
