namespace Battleship.Messages
{
    public interface IMessage
    {
        MessageTypeId TypeId { get; }
        TResult Accept<TResult>(IMessageVisitor<TResult> visitor);
    }
}
