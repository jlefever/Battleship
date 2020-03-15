using Battleship.Messages;

namespace Battleship
{
    /// <summary>
    /// Handle a message. Generally used as a callback.
    /// </summary>
    public interface IMessageHandler
    {
        void Handle(IMessage message);
    }
}
