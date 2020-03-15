using Battleship.DFA;
using Battleship.Loggers;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Battleship.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a logger
            var logger = new Logger(Console.Out);

            // If the user did not supply an IP or hostname, attempt to
            // discover a server on the same subnet.
            IPEndPoint endPoint;
            if (args.Length > 0)
            {
                if (IPAddress.TryParse(args[0], out var ip))
                {
                    endPoint = new IPEndPoint(ip, BspConstants.DefaultPort);
                }
                else
                {
                    // Maybe the user supplied a hostname not an IP
                    var addresses = Dns.GetHostAddresses(args[0]);

                    if (addresses.Length < 1)
                    {
                        Console.WriteLine("Could not find hostname.");
                        return;
                    }

                    endPoint = new IPEndPoint(addresses.Last(), BspConstants.DefaultPort);
                }
            }
            else
            {
                logger.LogInfo("Attempting to discover server end point...");
                endPoint = await DiscoverServerEndPoint(BspConstants.DefaultPort);
            }

            // Attempt to connect to the server.
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

            // Create a disconnecter that can clean up a connection.
            var disconnecter = new ClientDisconnecter(logger, socket);

            // Create a sender for sending to the server.
            var senderHandler = new MultiMessageHandler();
            var sender = new BspSender(socket, logger, unparser, senderHandler);
            
            // Create a prompter to handle user interaction.
            var prompter = new Prompter(sender);

            // Create a state machine
            var container = new ClientNetworkStateContainer(prompter);
            var context = new NetworkStateContext(container, disconnecter);

            // Register sending messages with our state machine.
            senderHandler.AddHandler(LoggingMessageHandler.ForSending(logger));
            senderHandler.AddHandler(new SentMessageHandler(context));

            // Register receiving messages with our state machine.
            var receiverHandler = new MultiMessageHandler();
            receiverHandler.AddHandler(LoggingMessageHandler.ForReceiving(logger));
            receiverHandler.AddHandler(new ReceiveMessageHandler(context));

            // Create a parser for our connection with the server
            var parser = new MessageParser(receiverHandler, new GameTypeRepository());
            var receiver = new BspReceiver(socket, disconnecter, parser, logger);

            // Begin receive messages and start the prompt.
            var receivingTask = receiver.StartReceivingAsync();
            prompter.PromptLogOn();
            await receivingTask;
        }

        /// <summary>
        /// Discover the server EndPoint. Will async wait forever if it cannot find one.
        /// </summary>
        /// <param name="port"></param>
        /// <returns>IPEndPoint of the server</returns>
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
