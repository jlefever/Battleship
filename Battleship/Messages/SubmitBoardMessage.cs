namespace Battleship.Messages
{
    public class SubmitBoardMessage : IMessage
    {
        public MessageTypeId TypeId => MessageTypeId.SubmitBoard;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitSubmitBoardMessage(this);
        }
    }
}