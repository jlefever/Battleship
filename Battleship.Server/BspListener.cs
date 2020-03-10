using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Battleship.Server
{
    public sealed class BspListener : IDisposable
    {
        private readonly Logger _logger;
        private readonly Socket _socket;

        public BspListener(Logger logger)
        {
            _logger = logger;
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public async IAsyncEnumerable<BspConnection> ListenAsync(IPEndPoint endPoint, int backlog = 120)
        {
            _socket.Bind(endPoint);
            _logger.LogInfo("Listening on " + _socket.LocalEndPoint);
            _socket.Listen(backlog);

            while (true)
            {
                var socket = await _socket.AcceptAsync();
                yield return new BspConnection(socket, _logger);
            }
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }
}
