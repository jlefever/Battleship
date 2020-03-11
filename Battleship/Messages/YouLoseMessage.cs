namespace Battleship.Messages
{
    public class YouLoseMessage : IMessage
    {
        public MessageTypeId TypeId => MessageTypeId.YouLose;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitYouLoseMessage(this);
        }
    }
}