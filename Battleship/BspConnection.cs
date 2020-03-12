using System;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using Battleship.Messages;

namespace Battleship
{
    public class BspConnection
    {
        private readonly Socket _socket;
        private readonly Logger _logger;

        public BspConnection(Socket socket, Logger logger)
        {
            _socket = socket;
            _logger = logger;
        }

        public async Task HandleAsync()
        {
            _logger.LogInfo("Connected to " + _socket.RemoteEndPoint);

            var parser = new MessageParser(new BasicMessageHandler());
            var stream = new NetworkStream(_socket);
            var reader = PipeReader.Create(stream);

            while (true)
            {
                var readResult = await reader.ReadAsync();
                var buffer = readResult.Buffer;
                var result = parser.Parse(buffer);

                if (result == null)
                {
                    break;
                }

                reader.AdvanceTo(result.Value, buffer.End);

                if (readResult.IsCompleted)
                {
                    break;
                }
            }

            await reader.CompleteAsync();
            _logger.LogInfo("Disconnected from " + _socket.RemoteEndPoint);
        }
    }

    public class BasicMessageHandler : IMessageHandler
    {
        public void Handle(IMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
