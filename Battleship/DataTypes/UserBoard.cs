namespace Battleship.DataTypes
{
    public class UserBoard
    {
        public UserBoard(string username, Board board)
        {
            Username = username;
            Board = board;
            AcceptedGame = MatchResponse.None;
        }

        public MatchResponse AcceptedGame { get; set; }

        public string Username { get; }
        public Board Board { get; }
    }
}
