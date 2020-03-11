namespace Battleship.Messages
{
    public class RejectBoardMessage : IMessage
    {
        public MessageTypeId TypeId => MessageTypeId.RejectBoard;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitRejectBoardMessage(this);
        }
    }
}