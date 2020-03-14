using System.Linq.Expressions;

namespace Battleship.DataTypes
{
    public class GameType
    {
        public GameType(byte gameTypeId, byte boardWidth, byte boardHeight, byte[] ships)
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

        public override string ToString()
        {
            return $"{nameof(GameTypeId)}: {GameTypeId}, " +
                   $"{nameof(BoardWidth)}: {BoardWidth}, " +
                   $"{nameof(BoardHeight)}: {BoardHeight}, " +
                   $"{nameof(Ships)}: {Ships}";
        }
    }
}