using Battleship.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Battleship.DFA;
using Battleship.Loggers;
using Battleship.Repositories;

namespace Battleship.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = new Logger(Console.Out);
            var unparser = new MessageUnparser();

            var endpoint = new IPEndPoint(IPAddress.Loopback, 9096);
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                logger.LogInfo($"Attempting to connect to {endpoint}...");
                socket.Connect(endpoint);
            }
            catch (SocketException)
            {
                logger.LogError($"Failed to connect to {endpoint}.");
            }

            var senderHandler = new MultiMessageHandler();
            var sender = new BspSender(socket, logger, unparser, senderHandler);

            var container = new ClientNetworkStateContainer(sender, logger);
            var context = new NetworkStateContext(container);

            senderHandler.AddHandler(new SentMessageHandler(context));

            var receiverHandler = new MultiMessageHandler();
            receiverHandler.AddHandler(new LoggingMessageHandler(logger));
            // receiverHandler.AddHandler(new ReceiveMessageHandler(context));

            var parser = new MessageParser(receiverHandler, new GameTypeRepository());
            var receiver = new BspReceiver(logger);
            _ = receiver.StartReceivingAsync(socket, parser);

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();

                if (input == "send")
                {
                    // var m = new LogOnMessage(0, "jason", "password");
                    await sender.SendAsync(new BasicMessage(MessageTypeId.Hit));
                }
            }
        }
    }
}
