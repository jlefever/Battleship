using Battleship.DataTypes;

namespace Battleship.Messages
{
    public class YouLoseMessage : IMessage
    {
        public YouLoseMessage(Position position)
        {
            Position = position;
        }

        public Position Position { get; }

        public MessageTypeId TypeId => MessageTypeId.YouLose;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitYouLoseMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(Position)}: {Position}";
        }
    }
}