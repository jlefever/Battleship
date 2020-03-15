﻿using Battleship.DFA;
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
            var unparser = new MessageUnparser();

            var endpoint = new IPEndPoint(IPAddress.Loopback, BspConstants.DefaultPort);
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                logger.LogInfo($"Attempting to connect to {endpoint}");
                socket.Connect(endpoint);
            }
            catch (SocketException)
            {
                logger.LogError($"Failed to connect to {endpoint}");
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
    }
}
