namespace Battleship.Messages
{
    public interface IMessageVisitor<out TResult>
    {
        public TResult VisitLogOn(LogOnMessage message);
        public TResult VisitRejectLogOn(RejectLogOn message);
        public TResult VisitAcceptLogOn(AcceptLogOn message);
    }
}
