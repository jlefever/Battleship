using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Battleship
{
    public sealed class BspListener
    {
        private readonly Logger _logger;

        public BspListener(Logger logger)
        {
            _logger = logger;
        }

        // TODO: Cancellation token?
        public async IAsyncEnumerable<Socket> BeginListeningAsync(IPEndPoint endPoint, int backlog = 120)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            _logger.LogInfo("Listening on " + socket.LocalEndPoint);
            socket.Listen(backlog);

            while (true)
            {
                yield return await socket.AcceptAsync();
            }

            socket.Dispose();
        }
    }
}
