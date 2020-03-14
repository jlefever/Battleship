using Battleship.Messages;

namespace Battleship
{
    public interface IMessageHandler
    {
        void Handle(IMessage message);
    }
}
