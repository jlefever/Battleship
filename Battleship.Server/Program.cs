using System;
using System.Net;
using System.Threading.Tasks;
using Battleship.DFA;
using Battleship.Loggers;
using Battleship.Repositories;

namespace Battleship.Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var ip = new IPEndPoint(IPAddress.Loopback, 9096);

            var generalLogger = new Logger(Console.Out);
            var unparser = new MessageUnparser();
            var listener = new BspListener(generalLogger);
            var receiver = new BspReceiver(generalLogger);

            await foreach (var socket in listener.StartListeningAsync(ip))
            {
                var logger = new EndPointLogger(Console.Out, socket.RemoteEndPoint);
                var sender = new BspSender(socket, logger, unparser);
                var container = new NetworkStateContainer(new UserRepository());
                var context = new NetworkStateContext(sender, container);

                var handler = new MultiMessageHandler();
                handler.AddHandler(new LoggingMessageHandler(logger));
                handler.AddHandler(new ServerMessageHandler(context));
                
                _ = receiver.StartReceivingAsync(socket, new MessageParser(handler));
            }
        }
    }
}
