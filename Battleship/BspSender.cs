using System;
using Battleship.Messages;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Battleship
{
    public sealed class BspSender : IDisposable
    {
        private readonly Socket _socket;
        private readonly NetworkStream _stream;
        private readonly Logger _logger;
        private readonly MessageUnparser _unparser;

        public BspSender(Socket socket, Logger logger, MessageUnparser unparser)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
            _logger = logger;
            _unparser = unparser;
        }

        // Todo: Cancellation token?
        public async Task SendAsync(IMessage message)
        {
            _logger.LogInfo($"Sending {message} to {_socket.RemoteEndPoint}...");
            await _stream.WriteAsync(message.Accept(_unparser).ToArray());
        }

        public void Dispose()
        {
            _stream.Dispose();
            _socket.Dispose();
        }
    }
}
