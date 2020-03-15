using Battleship.DataTypes;
using Battleship.DFA;
using Battleship.Loggers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Battleship.Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var ip = ReadArgs(args);

            var generalLogger = new Logger(Console.Out);
            var unparser = new MessageUnparser();
            var listener = new BspListener(generalLogger);
            var gameTypeRepo = CreateGameTypeRepository();
            var userRepo = CreateUserRepository();
            var matchMaker = new MatchMaker();

            await foreach (var socket in listener.StartListeningAsync(ip))
            {
                var logger = new EndPointLogger(Console.Out, socket.RemoteEndPoint);
                var disconnecter = new ServerDisconnecter(logger, socket, userRepo, matchMaker);
                var senderHandler = new MultiMessageHandler();
                var sender = new BspSender(socket, logger, unparser, senderHandler);
                var container = new ServerNetworkStateContainer(sender, disconnecter, gameTypeRepo,
                    userRepo, matchMaker);
                var context = new NetworkStateContext(container, disconnecter);
                senderHandler.AddHandler(LoggingMessageHandler.ForSending(logger));
                senderHandler.AddHandler(new SentMessageHandler(context));

                var receiverHandler = new MultiMessageHandler();
                receiverHandler.AddHandler(LoggingMessageHandler.ForReceiving(logger));
                receiverHandler.AddHandler(new ReceiveMessageHandler(context));

                var parser = new MessageParser(receiverHandler, gameTypeRepo);
                var receiver = new BspReceiver(socket, disconnecter, parser, logger);
                _ = receiver.StartReceivingAsync();
            }
        }

        private static IPEndPoint ReadArgs(IReadOnlyList<string> args)
        {
            switch (args.Count)
            {
                case 0:
                    return new IPEndPoint(IPAddress.Loopback, BspConstants.DefaultPort);
                case 1 when !args[0].Contains(':'):
                    return new IPEndPoint(IPAddress.Parse(args[0]), BspConstants.DefaultPort);
                case 1:
                    {
                        var input = args[0].Split(':');
                        var ip = IPAddress.Parse(input[0]);
                        var port = Convert.ToInt32(input[1]);
                        return new IPEndPoint(ip, port);
                    }
                default:
                    throw new Exception("Too many command line arguments!");
            }
        }

        private static GameTypeRepository CreateGameTypeRepository()
        {
            var repo = new GameTypeRepository();

            repo.TryAdd(new GameType(1, 15, 15, new byte[] { 2 }));
            repo.TryAdd(new GameType(2, 5, 5, new byte[] { 1 }));
            repo.TryAdd(new GameType(3, 10, 10, new byte[] { 3, 2, 1 }));

            return repo;
        }

        private static UserRepository CreateUserRepository()
        {
            var repo = new UserRepository();

            repo.TryAdd(new User("jason", "password"));
            repo.TryAdd(new User("sam", "password"));
            repo.TryAdd(new User("alice", "password"));
            repo.TryAdd(new User("bob", "password"));

            return repo;
        }
    }
}
