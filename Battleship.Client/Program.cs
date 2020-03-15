using Battleship.DFA;
using Battleship.Loggers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Battleship.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = new Logger(Console.Out);

            IPEndPoint endPoint;

            if (args.Length > 0)
            {
                endPoint = new IPEndPoint(IPAddress.Parse(args[0]), BspConstants.DefaultPort);
            }
            else
            {
                logger.LogInfo("Attempting to discover server end point...");
                endPoint = await DiscoverServerEndPoint(BspConstants.DefaultPort);
            }

            var unparser = new MessageUnparser();
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                logger.LogInfo($"Attempting to connect to {endPoint}");
                socket.Connect(endPoint);
            }
            catch (SocketException)
            {
                logger.LogError($"Failed to connect to {endPoint}");
            }

            var disconnecter = new ClientDisconnecter(logger, socket);
            var senderHandler = new MultiMessageHandler();
            var sender = new BspSender(socket, logger, unparser, senderHandler);
            var prompter = new Prompter(sender);
            var container = new ClientNetworkStateContainer(prompter);
            var context = new NetworkStateContext(container, disconnecter);

            senderHandler.AddHandler(LoggingMessageHandler.ForSending(logger));
            senderHandler.AddHandler(new SentMessageHandler(context));

            var receiverHandler = new MultiMessageHandler();
            receiverHandler.AddHandler(LoggingMessageHandler.ForReceiving(logger));
            receiverHandler.AddHandler(new ReceiveMessageHandler(context));

            var parser = new MessageParser(receiverHandler, new GameTypeRepository());
            var receiver = new BspReceiver(socket, disconnecter, parser, logger);

            var receivingTask = receiver.StartReceivingAsync();
            prompter.PromptLogOn();
            await receivingTask;
        }

        private static async Task<IPEndPoint> DiscoverServerEndPoint(int port)
        {
            // Create our broadcast address
            var broadcastIp = IPAddress.Parse("255.255.255.255");
            var broadcastEndpoint = new IPEndPoint(broadcastIp, port);

            // Create a UDP socket with broadcasting enabled
            var broadcaster = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            broadcaster.EnableBroadcast = true;

            // Connect to this socket to find our local IP
            broadcaster.Connect(broadcastEndpoint);
            var localIp = ((IPEndPoint)broadcaster.LocalEndPoint).Address;

            // Create a TCP socket for listening for the server IP
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind to our local IP we just discovered but with our standard port
            listener.Bind(new IPEndPoint(localIp, port));
            listener.Listen(120);

            // Send off our broadcast datagram
            broadcaster.SendTo(Array.Empty<byte>(), broadcastEndpoint);

            // Async wait for the server IP address to come on the TCP socket
            var buffer = new byte[4];
            var socket = await listener.AcceptAsync();
            await socket.ReceiveAsync(buffer, SocketFlags.None);

            // Release all resources used by our sockets
            broadcaster.Dispose();
            listener.Dispose();

            // Return the address we received from the server
            return new IPEndPoint(new IPAddress(buffer), port);
        }
    }
}
