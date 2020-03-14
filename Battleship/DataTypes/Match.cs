namespace Battleship.DataTypes
{
    public class Match
    {
        public Match(UserBoard player, UserBoard opponent)
        {
            Player = player;
            Opponent = opponent;
        }

        public UserBoard Player { get; }
        public UserBoard Opponent { get; }
    }
}
