namespace Battleship.Messages
{
    public class ShipLocation
    {
        public ShipLocation(byte rowIndex, byte colIndex, bool vertical)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
            Vertical = vertical;
        }

        public byte RowIndex { get; }
        public byte ColIndex { get; }
        public bool Vertical { get; }

        public override string ToString()
        {
            return $"{nameof(RowIndex)}: {RowIndex}, {nameof(ColIndex)}: " +
                   $"{ColIndex}, {nameof(Vertical)}: {Vertical}";
        }
    }
}