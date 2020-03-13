using System;
using System.IO;
using System.Net;

namespace Battleship.Loggers
{
    public class EndPointLogger : ILogger
    {
        private readonly TextWriter _writer;
        private readonly EndPoint _endpoint;

        public EndPointLogger(TextWriter writer, EndPoint endpoint)
        {
            _writer = writer;
            _endpoint = endpoint;
        }

        public void LogInfo(string message)
        {
            Log($"[{_endpoint}][INFO]", message);
        }

        public void LogWarn(string message)
        {
            Log($"[{_endpoint}][WARN]", message);
        }

        public void LogError(string message)
        {
            Log($"[{_endpoint}][ERROR]", message);
        }

        public void LogError(string message, Exception exception)
        {
            Log($"[{_endpoint}][ERROR]", message);
            _writer.WriteLine(exception);
        }

        private void Log(string severity, string message)
        {
            _writer.WriteLine($"[{DateTime.Now}]{severity} {message}");
        }
    }
}
