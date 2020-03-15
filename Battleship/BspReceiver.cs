using System;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using Battleship.Loggers;

namespace Battleship
{
    public sealed class BspReceiver : IDisposable
    {
        private readonly Socket _socket;
        private readonly NetworkStream _stream;
        private readonly ILogger _logger;
        private readonly IBspDisconnecter _disconnecter;
        private readonly MessageParser _parser;

        public BspReceiver(Socket socket, IBspDisconnecter disconnecter, MessageParser parser, ILogger logger)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
            _logger = logger;
            _disconnecter = disconnecter;
            _parser = parser;
        }

        public async Task StartReceivingAsync()
        {
            var reader = PipeReader.Create(_stream);
            _logger.LogInfo("Connected to " + _socket.RemoteEndPoint);

            while (true)
            {
                // Suspend this method until reader returns data.
                var result = await reader.ReadAsync();

                // Parse contents until we run out of valid & complete messages.
                var position = _parser.Parse(result.Buffer);

                // Disconnect if we receive any invalid data.
                if (position == null) break;

                // Tell the reader how much data we evaluated so it does not return
                // data we have already seen.
                reader.AdvanceTo(position.Value, result.Buffer.End);
            }

            await reader.CompleteAsync();
            _disconnecter.Disconnect();
        }

        public void Dispose()
        {
            _socket.Dispose();
            _stream.Dispose();
        }
    }
}
