using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using Battleship.DataTypes;

namespace Battleship.Repositories
{
    public class UserRepository
    {
        private readonly IDictionary<string, User> _users;
        private readonly IDictionary<string, BspSender> _senders;

        public UserRepository()
        {
            _users = new Dictionary<string, User>();
            _senders = new Dictionary<string, BspSender>();
        }

        public bool TryAdd(User user)
        {
            return _users.TryAdd(user.Username, user);
        }

        public bool TryLogIn(string username, string password, BspSender sender)
        {
            // Check if user exists
            if (!_users.TryGetValue(username, out var user))
            {
                return false;
            }

            // Validate password
            if (user.Password != password)
            {
                return false;
            }

            // Check if user is already logged in
            if (_senders.ContainsKey(user.Username))
            {
                return false;
            }

            // Add their connection to the dict.
            // This also denotes that they are currently logged in.
            _senders.Add(user.Username, sender);
            return true;
        }

        public bool TryGetSender(string username, out BspSender sender)
        {
            return _senders.TryGetValue(username, out sender);
        }

        public bool TryGetUsernameBySocket(Socket socket, out string username)
        {
            foreach (var sender in _senders)
            {
                if (sender.Value.Socket == socket)
                {
                    username = sender.Key;
                    return true;
                }
            }

            username = null;
            return false;
        }

        public void LogOut(string username)
        {
            _senders.Remove(username);
        }
    }
}
