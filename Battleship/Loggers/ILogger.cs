using System;

namespace Battleship.Loggers
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogError(string message);
        void LogError(string message, Exception exception);
    }
}