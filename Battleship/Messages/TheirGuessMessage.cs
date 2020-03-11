namespace Battleship.Messages
{
    public class TheirGuessMessage : IMessage
    {
        public MessageTypeId TypeId => MessageTypeId.TheirGuess;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitTheirGuessMessage(this);
        }
    }
}