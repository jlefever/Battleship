using Battleship.Loggers;
using Battleship.Messages;

namespace Battleship.DFA.Client
{
    public class NotConnected : INotConnected
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public NotConnected(BspSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public void Received(NetworkStateContext context, IMessage message)
        {
            // throw new NotImplementedException();
        }

        public void Sent(NetworkStateContext context, IMessage message)
        {
            // throw new NotImplementedException();
        }
    }
}
