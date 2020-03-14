using Battleship.Messages;
using System.Collections.Generic;

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

        public void Handle(IMessage message)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle(message);
            }
        }
    }
}
