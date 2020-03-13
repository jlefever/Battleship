namespace Battleship.Repositories
{
    public class UserRepository
    {
        public bool IsValidUser(string username, string password)
        {
            return username == "jason" && password == "password";
        }
    }
}
