using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Battleship
{
    public sealed class BspSender : IDisposable
    {
        private readonly Socket _socket;
        private readonly NetworkStream _stream;
        private readonly ILogger _logger;
        private readonly MessageUnparser _unparser;
        private readonly IMessageHandler _handler;

        public BspSender(Socket socket, ILogger logger, MessageUnparser unparser, IMessageHandler handler)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
            _logger = logger;
            _unparser = unparser;
            _handler = handler;
        }

        // Todo: Cancellation token?
        public void Send(IMessage message)
        {
            if (!_socket.Connected)
            {
                _logger.LogError($"Failed to send {message} on not connected socket");
                return;
            }

            _handler.Handle(message);
            _stream.WriteAsync(message.Accept(_unparser).ToArray());
        }

        public void Disconnect()
        {
            _socket.Disconnect(false);
        }

        public void Dispose()
        {
            _stream.Dispose();
            _socket.Dispose();
        }
    }
}
