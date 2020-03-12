using System;
using System.Net;
using System.Threading.Tasks;

namespace Battleship.Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var ip = new IPEndPoint(IPAddress.Loopback, 9096);

            var logger = new Logger(Console.Out);
            var unparser = new MessageUnparser();
            var listener = new BspListener(logger);
            var receiver = new BspReceiver(logger);

            await foreach (var socket in listener.StartListeningAsync(ip))
            {
                var sender = new BspSender(socket, logger, unparser);
                var handler = new PongMessageHandler(sender, logger);
                _ = receiver.StartReceivingAsync(socket, new MessageParser(handler));
            }
        }
    }
}
