namespace Battleship.Messages
{
    public interface IMessageVisitor<out TResult>
    {
        public TResult VisitBasicMessage(BasicMessage message);
        public TResult VisitLogOnMessage(LogOnMessage message);
        public TResult VisitRejectLogOnMessage(RejectLogOnMessage message);
        public TResult VisitGameTypeMessage(GameTypeMessage message);
        public TResult VisitSubmitBoardMessage(SubmitBoardMessage message);
        public TResult VisitRejectBoardMessage(RejectBoardMessage message);
        public TResult VisitMyGuessMessage(MyGuessMessage message);
        public TResult VisitTheirGuessMessage(TheirGuessMessage message);
        public TResult VisitYouLoseMessage(YouLoseMessage message);
    }
}
