using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using Battleship.Loggers;

namespace Battleship
{
    public class BspReceiver
    {
        private readonly ILogger _logger;

        public BspReceiver(ILogger logger)
        {
            _logger = logger;
        }

        public async Task StartReceivingAsync(Socket socket, MessageParser parser)
        {
            var reader = PipeReader.Create(new NetworkStream(socket));
            _logger.LogInfo("Connected to " + socket.RemoteEndPoint);

            while (true)
            {
                // Suspend this method until reader returns data.
                var result = await reader.ReadAsync();

                // Parse contents until we run out of valid & complete messages.
                var position = parser.Parse(result.Buffer);

                // Disconnect if we receive any invalid data.
                if (position == null) break;

                // Tell the reader how much data we evaluated so it does not return
                // data we have already seen.
                reader.AdvanceTo(position.Value, result.Buffer.End);
            }

            await reader.CompleteAsync();
            socket.Disconnect(false);
            _logger.LogInfo("Disconnected from " + socket.RemoteEndPoint);
        }
    }
}
