namespace Battleship.Messages
{
    public class MyGuessMessage : IMessage
    {
        public MessageTypeId TypeId => MessageTypeId.MyGuess;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitMyGuessMessage(this);
        }
    }
}