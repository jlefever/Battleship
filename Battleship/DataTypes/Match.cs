namespace Battleship.DataTypes
{
    public class Match
    {
        public Match(UserBoard player, UserBoard opponent, bool playerGoesFirst)
        {
            Player = player;
            Opponent = opponent;
            PlayerGoesFirst = playerGoesFirst;
        }

        public UserBoard Player { get; }
        public UserBoard Opponent { get; }
        public bool PlayerGoesFirst { get; }
    }
}
