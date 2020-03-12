using System.Threading.Tasks;
using Battleship.Messages;

namespace Battleship
{
    public interface IMessageHandler
    {
        Task HandleAsync(IMessage message);
    }
}
