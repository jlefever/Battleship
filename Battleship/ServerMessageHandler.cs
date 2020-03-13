using Battleship.Messages;
using System.Threading.Tasks;
using Battleship.DFA;

namespace Battleship
{
    public class ServerMessageHandler : IMessageHandler
    {
        private readonly NetworkStateContext _context;

        public ServerMessageHandler(NetworkStateContext context)
        {
            _context = context;
        }

        public Task HandleAsync(IMessage message)
        {
            _context.ServerReceived(message);
            return Task.CompletedTask;
        }
    }
}
