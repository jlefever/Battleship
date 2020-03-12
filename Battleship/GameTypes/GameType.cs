namespace Battleship.GameTypes
{
    public class GameType
    {
        public GameType(byte boardHeight, byte boardWidth, byte[] ships)
        {
            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            Ships = ships;
        }

        public byte BoardHeight { get; }
        public byte BoardWidth { get; }
        public byte[] Ships { get; }
    }
}
