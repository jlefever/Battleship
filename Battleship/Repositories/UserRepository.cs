using System.Collections.Generic;
using Battleship.DataTypes;

namespace Battleship.Repositories
{
    public class UserRepository
    {
        private readonly IDictionary<string, User> _users;
        private readonly ISet<string> _loggedInUsers;

        public UserRepository()
        {
            _users = new Dictionary<string, User>();
            _loggedInUsers = new HashSet<string>();
        }

        public bool TryAdd(User user)
        {
            return _users.TryAdd(user.Username, user);
        }

        public bool TryLogIn(string username, string password)
        {
            if (!_users.TryGetValue(username, out var user))
            {
                // User does not exist
                return false;
            }

            if (user.Password != password)
            {
                // Wrong password
                return false;
            }

            if (_loggedInUsers.Contains(user.Username))
            {
                // Already logged in
                return false;
            }

            _loggedInUsers.Add(user.Username);
            return true;
        }

        public void LogOut(string username)
        {
            _loggedInUsers.Remove(username);
        }

        // Maintain a list//dict of BspSenders
    }
}
