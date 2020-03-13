using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Battleship.Loggers;

namespace Battleship
{
    public sealed class BspListener
    {
        private readonly ILogger _logger;

        public BspListener(ILogger logger)
        {
            _logger = logger;
        }

        // TODO: Cancellation token?
        public async IAsyncEnumerable<Socket> StartListeningAsync(IPEndPoint endPoint, int backlog = 120)
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
