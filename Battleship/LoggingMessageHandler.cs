using Battleship.Messages;
using System.Threading.Tasks;
using Battleship.Loggers;

namespace Battleship
{
    public class LoggingMessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;

        public LoggingMessageHandler(ILogger logger)
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