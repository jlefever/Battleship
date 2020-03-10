namespace Battleship.Messages
{
    public class AcceptLogOn : IMessage
    {
        public short MessageKind => 2;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitAcceptLogOn(this);
        }
    }
}
