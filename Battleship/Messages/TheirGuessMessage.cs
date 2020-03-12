namespace Battleship.Messages
{
    public class TheirGuessMessage : IMessage
    {
        public TheirGuessMessage(byte rowIndex, byte colIndex)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }

        public byte RowIndex { get; }
        public byte ColIndex { get; }

        public MessageTypeId TypeId => MessageTypeId.TheirGuess;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitTheirGuessMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(RowIndex)}: " +
                   $"{RowIndex}, {nameof(ColIndex)}: {ColIndex}";
        }
    }
}