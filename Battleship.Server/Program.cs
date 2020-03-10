using System;
using System.Net;
using System.Threading.Tasks;

namespace Battleship.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            var ip = new IPEndPoint(IPAddress.Loopback, 9096);
            var listener = new BspListener(new Logger(Console.Out));

            await foreach (var connection in listener.ListenAsync(ip))
            {
                _ = connection.HandleAsync();
            }

            listener.Dispose();
        }
    }
}
