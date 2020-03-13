using Battleship.DataTypes;

namespace Battleship.Messages
{
    public class MyGuessMessage : IMessage
    {
        public MyGuessMessage(Position position)
        {
            Position = position;
        }

        public Position Position { get; }

        public MessageTypeId TypeId => MessageTypeId.MyGuess;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitMyGuessMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(Position)}: {Position}";
        }
    }
}