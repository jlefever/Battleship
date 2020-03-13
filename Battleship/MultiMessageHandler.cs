using Battleship.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship
{
    public class MultiMessageHandler : IMessageHandler
    {
        private readonly IList<IMessageHandler> _handlers;

        public MultiMessageHandler()
        {
            _handlers = new List<IMessageHandler>();
        }

        public void AddHandler(IMessageHandler handler)
        {
            _handlers.Add(handler);
        }

        public Task HandleAsync(IMessage message)
        {
            foreach (var handler in _handlers)
            {
                handler.HandleAsync(message);
            }

            return Task.CompletedTask;
        }
    }
}
