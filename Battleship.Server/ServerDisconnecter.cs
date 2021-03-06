﻿using Battleship.Loggers;
using System.Net.Sockets;

namespace Battleship.Server
{
    /// <summary>
    /// Manage disconnecting from a client.
    /// </summary>
    public class ServerDisconnecter : IBspDisconnecter
    {
        private readonly ILogger _logger;
        private readonly Socket _socket;
        private readonly UserRepository _userRepo;
        private readonly MatchMaker _matchMaker;

        public ServerDisconnecter(ILogger logger, Socket socket, UserRepository userRepo, MatchMaker matchMaker)
        {
            _logger = logger;
            _socket = socket;
            _userRepo = userRepo;
            _matchMaker = matchMaker;
        }

        /// <summary>
        /// Disconnect from a client. Log the user out and remove them from matchmaking.
        /// </summary>
        public void Disconnect()
        {
            _logger.LogInfo($"Disconnecting from {_socket.RemoteEndPoint}");
            _socket.Disconnect(false);

            if (!_userRepo.TryGetUsernameBySocket(_socket, out var username))
            {
                _logger.LogError("Client disconnected but did not sign out.");
                return;
            }

            _matchMaker.Remove(username);
            _userRepo.LogOut(username);
        }
    }
}
