using Battleship.Messages;
using System.Threading.Tasks;

namespace Battleship
{
    public class PrintingMessageHandler : IMessageHandler
    {
        private readonly Logger _logger;

        public PrintingMessageHandler(Logger logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(IMessage message)
        {
            _logger.LogInfo("Received " + message);
            return Task.CompletedTask;
        }
    }
}