using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Battleship.Messages;

namespace Battleship.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            var a = GetSampleLogOnBytes();
            var b = GetSampleLogOnBytes2();

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Connecting to port 8087...");
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Loopback, 9096));
                var stream = new NetworkStream(socket);

                while (true)
                {
                    Console.Write(">> ");

                    if (Console.ReadLine() == "send")
                    {
                        await stream.WriteAsync(GetSampleLogOnBytes().Take(20).ToArray());
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Network problem.");
            }
        }

        private static void Run(string readLine)
        {
            throw new NotImplementedException();
        }

        private static byte[] GetSampleLogOnBytes()
        {
            const short id = 0;
            const byte extension = 1;
            const byte version = 2;
            var username = "jason".PadRight(16, '\0');
            var password = "password".PadRight(16, '\0');

            var idBytes = BitConverter.GetBytes(id);
            var extensionBytes = new[] { extension };
            var versionBytes = new[] { version };
            var usernameBytes = Encoding.ASCII.GetBytes(username);
            var passwordBytes = Encoding.ASCII.GetBytes(password);

            return idBytes
                .Concat(extensionBytes)
                .Concat(versionBytes)
                .Concat(usernameBytes)
                .Concat(passwordBytes)
                .ToArray();
        }

        private static byte[] GetSampleLogOnBytes2()
        {
            var message = new LogOnMessage(0, "jason", "lefever");
            return message.Accept(new MessageUnparser());
        }
    }
}
