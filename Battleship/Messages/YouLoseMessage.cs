namespace Battleship.Messages
{
    public class YouLoseMessage : IMessage
    {
        public YouLoseMessage(byte rowIndex, byte colIndex)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }

        public byte RowIndex { get; }
        public byte ColIndex { get; }

        public MessageTypeId TypeId => MessageTypeId.YouLose;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitYouLoseMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(RowIndex)}: {RowIndex}" +
                   $", {nameof(ColIndex)}: {ColIndex}";
        }
    }
}