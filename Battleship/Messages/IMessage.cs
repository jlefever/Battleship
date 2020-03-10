namespace Battleship.Messages
{
    public interface IMessage
    {
        TResult Accept<TResult>(IMessageVisitor<TResult> visitor);
    }
}
