using Battleship.Messages;
using System.Threading.Tasks;
using Battleship.Loggers;

namespace Battleship
{
    public class PongMessageHandler : IMessageHandler
    {
        private readonly BspSender _sender;
        private readonly ILogger _logger;

        public PongMessageHandler(BspSender sender, ILogger logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task HandleAsync(IMessage message)
        {
            _logger.LogInfo("Received " + message);
            await Task.Delay(1000);
            _ = _sender.SendAsync(new BasicMessage(MessageTypeId.YouWin));
        }
    }
}
