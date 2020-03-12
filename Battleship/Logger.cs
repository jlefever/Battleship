using System;
using System.IO;

namespace Battleship
{
    public class Logger
    {
        private readonly TextWriter _writer;

        public Logger(TextWriter writer)
        {
            _writer = writer;
        }

        public void LogInfo(string message)
        {
            Log("[INFO]", message);
        }

        public void LogWarn(string message)
        {
            Log("[WARN]", message);
        }

        public void LogError(string message)
        {
            Log("[ERROR]", message);
        }

        public void LogError(string message, Exception exception)
        {
            Log("[ERROR]", message);
            _writer.WriteLine(exception);
        }

        private void Log(string severity, string message)
        {
            _writer.WriteLine($"[{DateTime.Now}]{severity} {message}");
        }
    }
}
