using Battleship.DataTypes;
using Battleship.Loggers;
using Battleship.Messages;
using Battleship.Repositories;
using System;
using System.Collections.Generic;
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

            var senderHandler = new MultiMessageHandler();
            var sender = new BspSender(socket, logger, unparser, senderHandler);

            // var container = new ClientNetworkStateContainer(sender, logger);
            // var context = new NetworkStateContext(container);

            // senderHandler.AddHandler(new SentMessageHandler(context));

            var receiverHandler = new MultiMessageHandler();
            receiverHandler.AddHandler(LoggingMessageHandler.ForReceiving(logger));
            // receiverHandler.AddHandler(new ReceiveMessageHandler(context));

            var parser = new MessageParser(receiverHandler, new GameTypeRepository());
            var receiver = new BspReceiver(logger);
            _ = receiver.StartReceivingAsync(socket, parser);

            // REPL

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();

                if (input.StartsWith("logon"))
                {
                    var username = input.Split(' ')[1];
                    sender.Send(new LogOnMessage(BspConstants.Version, username, "password"));
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

                if (input == "acceptgame")
                {
                    sender.Send(new BasicMessage(MessageTypeId.AcceptGame));
                }

                if (input == "rejectgame")
                {
                    sender.Send(new BasicMessage(MessageTypeId.RejectGame));
                }

                if (input.StartsWith("guess"))
                {
                    var strings = input.Split(' ');

                    var row = Convert.ToByte(strings[1]);
                    var col = Convert.ToByte(strings[2]);

                    sender.Send(new MyGuessMessage(new Position(row, col)));
                }
            }
        }
    }
}
