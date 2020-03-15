using Battleship.Loggers;
using Battleship.Repositories;
using System.Net.Sockets;

namespace Battleship.Client
{
    public class ClientDisconnecter : IBspDisconnecter
    {
        private readonly ILogger _logger;
        private readonly Socket _socket;

        public ClientDisconnecter(ILogger logger, Socket socket)
        {
            _logger = logger;
            _socket = socket;
        }

        public void Disconnect()
        {
            _logger.LogInfo($"Disconnecting from {_socket.RemoteEndPoint}");
            _socket.Disconnect(false);
        }
    }
}
