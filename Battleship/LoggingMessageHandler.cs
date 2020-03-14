using Battleship.Loggers;
using Battleship.Messages;

namespace Battleship
{
    public class LoggingMessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;

        public LoggingMessageHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(IMessage message)
        {
            _logger.LogInfo("Received " + message);
        }
    }
}