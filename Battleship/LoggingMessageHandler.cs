using Battleship.Loggers;
using Battleship.Messages;

namespace Battleship
{
    /// <summary>
    /// Responsible for logging incoming and outgoing messages.
    /// </summary>
    public class LoggingMessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;
        private readonly bool _forSending;

        private LoggingMessageHandler(ILogger logger, bool forSending)
        {
            _logger = logger;
            _forSending = forSending;
        }

        /// <summary>
        /// Create a LoggingMessageHandler for sending messages
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <returns></returns>
        public static LoggingMessageHandler ForSending(ILogger logger)
        {
            return new LoggingMessageHandler(logger, true);
        }

        /// <summary>
        /// Create a LoggingMessageHandler for receiving messages
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <returns></returns>
        public static LoggingMessageHandler ForReceiving(ILogger logger)
        {
            return new LoggingMessageHandler(logger, false);
        }

        /// <summary>
        /// Log a sent or received message
        /// </summary>
        /// <param name="message"></param>
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