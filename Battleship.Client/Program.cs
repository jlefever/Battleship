using Battleship.Messages;
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

            var sender = new BspSender(socket, logger, unparser);
            var handler = new PongMessageHandler(sender, logger);
            var parser = new MessageParser(handler);
            var receiver = new BspReceiver(logger);
            _ = receiver.StartReceivingAsync(socket, parser);

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();

                if (input == "send")
                {
                    var message = new BasicMessage(MessageTypeId.YouWin);
                    await sender.SendAsync(message);
                }
            }
        }
    }
}
