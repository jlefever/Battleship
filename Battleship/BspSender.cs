using Battleship.Loggers;
using Battleship.Messages;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Battleship
{
    public sealed class BspSender : IDisposable
    {
        private readonly NetworkStream _stream;
        private readonly ILogger _logger;
        private readonly MessageUnparser _unparser;
        private readonly IMessageHandler _handler;

        public BspSender(Socket socket, ILogger logger, MessageUnparser unparser, IMessageHandler handler)
        {
            _stream = new NetworkStream(socket);
            _logger = logger;
            _unparser = unparser;
            _handler = handler;

            Socket = socket;
        }

        public Socket Socket { get; }

        // Todo: Cancellation token?
        public void Send(IMessage message)
        {
            if (!Socket.Connected)
            {
                _logger.LogError($"Failed to send {message} on not connected socket");
                return;
            }

            _handler.Handle(message);
            _stream.WriteAsync(message.Accept(_unparser).ToArray());
        }

        public void Dispose()
        {
            _stream.Dispose();
            Socket.Dispose();
        }
    }
}
