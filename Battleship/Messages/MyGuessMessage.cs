namespace Battleship.Messages
{
    public class MyGuessMessage : IMessage
    {
        public MyGuessMessage(byte rowIndex, byte colIndex)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }

        public byte RowIndex { get; set; }
        public byte ColIndex { get; set; }

        public MessageTypeId TypeId => MessageTypeId.MyGuess;

        public TResult Accept<TResult>(IMessageVisitor<TResult> visitor)
        {
            return visitor.VisitMyGuessMessage(this);
        }

        public override string ToString()
        {
            return $"{nameof(TypeId)}: {TypeId}, {nameof(RowIndex)}: " +
                   $"{RowIndex}, {nameof(ColIndex)}: {ColIndex}";
        }
    }
}