namespace Battleship.DataTypes
{
    public class UserBoard
    {
        public UserBoard(string username, Board board)
        {
            Username = username;
            Board = board;

            MatchResponse = MatchResponse.None;
        }

        public MatchResponse MatchResponse { get; set; }

        public string Username { get; }
        public Board Board { get; }
    }
}
