using Battleship.DataTypes;
using Battleship.DFA;
using Battleship.Loggers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Battleship.Server
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Must include the IP address to listen on as the only arguement.");
            }

            var localIp = IPAddress.Parse(args[0]);
            var localEndPoint = new IPEndPoint(localIp, BspConstants.DefaultPort);
            _ = StartUdpListing(localEndPoint);

            var generalLogger = new Logger(Console.Out);
            var unparser = new MessageUnparser();
            var listener = new BspListener(generalLogger);
            var gameTypeRepo = CreateGameTypeRepository();
            var userRepo = CreateUserRepository();
            var matchMaker = new MatchMaker();

            await foreach (var socket in listener.StartListeningAsync(localEndPoint))
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

        private static async Task StartUdpListing(IPEndPoint endPoint)
        {
            // Create a UDP client (wrapper around socket) for listening for broadcasts
            using var listener = new UdpClient(endPoint.Port);
            listener.EnableBroadcast = true;

            while (true)
            {
                // Async wait for a broadcast to be received
                var result = await listener.ReceiveAsync();

                // Use the IP of the received broadcast and the port of the server to make an endpoint
                var remoteEndPoint = new IPEndPoint(result.RemoteEndPoint.Address, endPoint.Port);

                // Create a TCP socket
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the device that sent the broadcast
                socket.Connect(remoteEndPoint);

                // Send the server IP to them
                socket.Send(endPoint.Address.GetAddressBytes());

                // Release the TCP socket
                socket.Dispose();
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
