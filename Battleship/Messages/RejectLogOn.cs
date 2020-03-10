namespace Battleship.Messages
{
    public class RejectLogOn : IMessage
    {
        public short MessageKind => 1;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitRejectLogOn(this);
        }
    }
}
