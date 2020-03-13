using Battleship.Messages;
using System.Threading.Tasks;
using Battleship.DFA;

namespace Battleship
{
    public class ServerMessageHandler : IMessageHandler
    {
        private readonly NetworkStateContext _context;
        private readonly Logger _logger;

        public ServerMessageHandler(NetworkStateContext context, Logger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task HandleAsync(IMessage message)
        {
            _logger.LogInfo("Received " + message);
            _context.ServerReceived(message);
        }
    }
}
