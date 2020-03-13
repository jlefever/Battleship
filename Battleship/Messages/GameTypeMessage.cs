using Battleship.DataTypes;

namespace Battleship.Messages
{
    public class GameTypeMessage : IMessage
    {
        public GameTypeMessage(GameType gameType)
        {
            GameType = gameType;
        }

        public GameType GameType { get; }

        public MessageTypeId TypeId => MessageTypeId.GameType;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitGameTypeMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(GameType)}: {GameType}";
        }
    }
}