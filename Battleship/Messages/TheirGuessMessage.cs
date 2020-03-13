using Battleship.DataTypes;

namespace Battleship.Messages
{
    public class TheirGuessMessage : IMessage
    {
        public TheirGuessMessage(Position position)
        {
            Position = position;
        }

        public Position Position { get; }

        public MessageTypeId TypeId => MessageTypeId.TheirGuess;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitTheirGuessMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(Position)}: {Position}";
        }
    }
}