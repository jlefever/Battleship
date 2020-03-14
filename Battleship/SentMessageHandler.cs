using Battleship.DFA;
using Battleship.Messages;

namespace Battleship
{
    public class SentMessageHandler : IMessageHandler
    {
        private readonly NetworkStateContext _context;

        public SentMessageHandler(NetworkStateContext context)
        {
            _context = context;
        }

        public void Handle(IMessage message)
        {
            _context.Sent(message);
        }
    }
}
