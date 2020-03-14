using Battleship.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Battleship.DataTypes;
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

            // var container = new ClientNetworkStateContainer(sender, logger);
            // var context = new NetworkStateContext(container);

            // senderHandler.AddHandler(new SentMessageHandler(context));

            var receiverHandler = new MultiMessageHandler();
            receiverHandler.AddHandler(new LoggingMessageHandler(logger));
            // receiverHandler.AddHandler(new ReceiveMessageHandler(context));

            var parser = new MessageParser(receiverHandler, new GameTypeRepository());
            var receiver = new BspReceiver(logger);
            _ = receiver.StartReceivingAsync(socket, parser);

            // REPL

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();

                if (input == "logon")
                {
                    sender.Send(new LogOnMessage(BspConstants.Version, "jason", "password"));
                }

                if (input == "submitboard")
                {
                    var placements = new List<Placement>
                    {
                        new Placement(new Position(0, 0), false),
                        new Placement(new Position(1, 0), false),
                        new Placement(new Position(2, 0), false),
                        new Placement(new Position(3, 0), false),
                        new Placement(new Position(4, 0), false)
                    };

                    sender.Send(new SubmitBoardMessage(0, placements));
                }

                if (input == "recallboard")
                {
                    sender.Send(new BasicMessage(MessageTypeId.RecallBoard));
                }
            }
        }
    }
}
