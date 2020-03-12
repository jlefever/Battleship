namespace Battleship.Messages
{
    public class GameTypeMessage : IMessage
    {
        public GameTypeMessage(byte gameTypeId, byte boardWidth, byte boardHeight, byte[] ships)
        {
            GameTypeId = gameTypeId;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            Ships = ships;
        }

        public byte GameTypeId { get; }
        public byte BoardWidth { get; }
        public byte BoardHeight { get; }
        public byte[] Ships { get; }

        public MessageTypeId TypeId => MessageTypeId.GameType;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitGameTypeMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(GameTypeId)}: " +
                   $"{GameTypeId}, {nameof(BoardWidth)}: {BoardWidth}, " +
                   $"{nameof(BoardHeight)}: {BoardHeight}, {nameof(Ships)}: " +
                   $"{Ships}";
        }
    }
}