using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Battleship
{
    public class BspReceiver
    {
        private readonly Logger _logger;

        public BspReceiver(Logger logger)
        {
            _logger = logger;
        }

        public async Task StartReceivingAsync(Socket socket, MessageParser parser)
        {
            var reader = PipeReader.Create(new NetworkStream(socket));
            _logger.LogInfo("Connected to " + socket.RemoteEndPoint);

            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;
                var position = parser.Parse(buffer);

                if (position == null)
                {
                    break;
                }

                reader.AdvanceTo(position.Value, buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await reader.CompleteAsync();
            _logger.LogInfo("Disconnected from " + socket.RemoteEndPoint);
        }
    }
}
