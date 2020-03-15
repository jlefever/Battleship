using Battleship.Loggers;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Battleship
{
    public sealed class BspListener
    {
        private readonly ILogger _logger;

        public BspListener(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// /Bind to an endpoint and async return any connections that attempt to connect.
        /// </summary>
        /// <param name="endPoint">The endpoint to bind to</param>
        /// <param name="backlog"></param>
        /// <returns>An async list of Sockets</returns>
        public async IAsyncEnumerable<Socket> StartListeningAsync(IPEndPoint endPoint, int backlog = 120)
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            _logger.LogInfo("Listening on " + socket.LocalEndPoint);
            socket.Listen(backlog);

            while (true)
            {
                yield return await socket.AcceptAsync();
            }
        }
    }
}
