using System;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using Battleship.Messages;

namespace Battleship.Server
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

            var stream = new NetworkStream(_socket);
            var reader = PipeReader.Create(stream);
            var badMessage = false;

            while (true)
            {
                var readResult = await reader.ReadAsync();
                var buffer = readResult.Buffer;

                while (true)
                {
                    var parser = new MessageParser(buffer);
                    var parseResult = parser.ParseMessage();

                    if (parseResult is BadParseResult)
                    {
                        badMessage = true;
                        break;
                    }

                    if (parseResult is IncompleteParseResult)
                    {
                        break;
                    }

                    if (!(parseResult is OkParseResult ok))
                    {
                        throw new Exception("Parser returned unknown ParseResult.");
                    }

                    buffer = buffer.Slice(parser.Current);
                    ProcessMessage(ok.Message);
                }

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (badMessage || readResult.IsCompleted)
                {
                    break;
                }
            }

            await reader.CompleteAsync();
            _logger.LogInfo("Disconnected from " + _socket.RemoteEndPoint);
        }

        private static void ProcessMessage(IMessage message)
        {
            Console.WriteLine(message);
        }
    }
}
