using System;
using System.Net;
using System.Threading.Tasks;
using Battleship.DataTypes;
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
            var gameTypeRepository = CreateGameTypeRepository();

            await foreach (var socket in listener.StartListeningAsync(ip))
            {
                var logger = new EndPointLogger(Console.Out, socket.RemoteEndPoint);
                var senderHandler = new MultiMessageHandler();
                var sender = new BspSender(socket, logger, unparser, senderHandler);
                var container = new ServerNetworkStateContainer(sender, logger, new UserRepository());
                var context = new NetworkStateContext(container);

                // I don't even want to do this. Just testing.
                senderHandler.AddHandler(new SentMessageHandler(context));

                var receiverHandler = new MultiMessageHandler();
                receiverHandler.AddHandler(new LoggingMessageHandler(logger));
                receiverHandler.AddHandler(new ReceiveMessageHandler(context));
                
                _ = receiver.StartReceivingAsync(socket, new MessageParser(receiverHandler, gameTypeRepository));
            }
        }

        private static GameTypeRepository CreateGameTypeRepository()
        {
            var repo = new GameTypeRepository();

            repo.TryAdd(new GameType(1, 15, 15, new byte[] { 2 }));
            repo.TryAdd(new GameType(2, 5, 5, new byte[] { 1 }));
            repo.TryAdd(new GameType(3, 10, 10, new byte[] {3, 2, 1}));

            return repo;
        }
    }
}
