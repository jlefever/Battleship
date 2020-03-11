namespace Battleship.Messages
{
    public class GameTypeMessage : IMessage
    {
        public MessageTypeId TypeId => MessageTypeId.GameType;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitGameTypeMessage(this);
        }
    }
}
