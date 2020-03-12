namespace Battleship.Messages
{
    public class RejectBoardMessage : IMessage
    {
        public RejectBoardMessage(RejectBoardErrorId errorId)
        {
            ErrorId = errorId;
        }

        public RejectBoardErrorId ErrorId { get; }

        public MessageTypeId TypeId => MessageTypeId.RejectBoard;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitRejectBoardMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(ErrorId)}: {ErrorId}";
        }
    }
}