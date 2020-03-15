using System.Collections.Generic;

namespace Battleship.DataTypes
{
    /// <summary>
    /// A configuration of the game Battleship.
    /// </summary>
    public class GameType
    {
        public GameType(byte gameTypeId, byte boardWidth, byte boardHeight, IList<byte> shipLengths)
        {
            GameTypeId = gameTypeId;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            ShipLengths = shipLengths;
        }

        public byte GameTypeId { get; }
        public byte BoardWidth { get; }
        public byte BoardHeight { get; }
        public IList<byte> ShipLengths { get; }

        public override string ToString()
        {
            return $"{nameof(GameTypeId)}: {GameTypeId}, " +
                   $"{nameof(BoardWidth)}: {BoardWidth}, " +
                   $"{nameof(BoardHeight)}: {BoardHeight}, " +
                   $"{nameof(ShipLengths)}: {ShipLengths}";
        }
    }
}