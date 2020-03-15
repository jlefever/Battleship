using Battleship.Loggers;
using Battleship.Messages;

namespace Battleship
{
    public class LoggingMessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;
        private readonly bool _forSending;

        private LoggingMessageHandler(ILogger logger, bool forSending)
        {
            _logger = logger;
            _forSending = forSending;
        }

        public static LoggingMessageHandler ForSending(ILogger logger)
        {
            return new LoggingMessageHandler(logger, true);
        }

        public static LoggingMessageHandler ForReceiving(ILogger logger)
        {
            return new LoggingMessageHandler(logger, false);
        }

        public void Handle(IMessage message)
        {
            if (_forSending)
            {
                _logger.LogInfo("Sending " + message);
                return;
            }

            _logger.LogInfo("Received " + message);
        }
    }
}