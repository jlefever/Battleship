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
                return;
            }

            // Require the IP address be supplied as a command line argument.
            var localIp = IPAddress.Parse(args[0]);
            var localEndPoint = new IPEndPoint(localIp, BspConstants.DefaultPort);

            // Begin listening for clients attempting to discover the server IP
            _ = StartUdpListing(localEndPoint);

            // Create objects used for every connection.
            var generalLogger = new Logger(Console.Out);
            var unparser = new MessageUnparser();
            var listener = new BspListener(generalLogger);
            var gameTypeRepo = CreateGameTypeRepository();
            var userRepo = CreateUserRepository();
            var matchMaker = new MatchMaker();

            // The body of this foreach loop is a callback. This body will run for
            // connection our listener accepts. Here we create objects specific for
            // each connection.
            await foreach (var socket in listener.StartListeningAsync(localEndPoint))
            {
                // Create a logger that will log with the client's EndPoint.
                var logger = new EndPointLogger(Console.Out, socket.RemoteEndPoint);

                // Create a disconnecter that can clean up a connection.
                var disconnecter = new ServerDisconnecter(logger, socket, userRepo, matchMaker);

                // Create a sender for sending messages to the client.
                var senderHandler = new MultiMessageHandler();
                var sender = new BspSender(socket, logger, unparser, senderHandler);

                // Create a container for this connections state.
                var container = new ServerNetworkStateContainer(sender, disconnecter, gameTypeRepo,
                    userRepo, matchMaker);

                // Create a context for our state machine.
                var context = new NetworkStateContext(container, disconnecter);

                // Register incoming messages with this context.
                senderHandler.AddHandler(LoggingMessageHandler.ForSending(logger));
                senderHandler.AddHandler(new SentMessageHandler(context));

                // Register outgoing messages with this context.
                var receiverHandler = new MultiMessageHandler();
                receiverHandler.AddHandler(LoggingMessageHandler.ForReceiving(logger));
                receiverHandler.AddHandler(new ReceiveMessageHandler(context));

                // Create a parser for this connection
                var parser = new MessageParser(receiverHandler, gameTypeRepo);

                // Begin asynchronously receiving messages
                var receiver = new BspReceiver(socket, disconnecter, parser, logger);
                _ = receiver.StartReceivingAsync();
            }
        }

        /// <summary>
        /// Will open a UDP socket listing for broadcasts. Responds by attempting to open
        /// a TCP connection with the client and sending them the server IP.
        /// </summary>
        /// <param name="endPoint">The server EndPoint</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a basic repository for GameTypes
        /// </summary>
        /// <returns>A small GameTypeRepository</returns>
        private static GameTypeRepository CreateGameTypeRepository()
        {
            var repo = new GameTypeRepository();

            repo.TryAdd(new GameType(1, 15, 15, new byte[] { 2 }));
            repo.TryAdd(new GameType(2, 5, 5, new byte[] { 1 }));
            repo.TryAdd(new GameType(3, 10, 10, new byte[] { 3, 2, 1 }));

            return repo;
        }

        /// <summary>
        /// Create a basic repository for Users
        /// </summary>
        /// <returns>A small UserRepository</returns>
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
