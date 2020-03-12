using System;

namespace Battleship
{
    public class MessageUnparserException : Exception
    {
        public MessageUnparserException() { }

        public MessageUnparserException(string message) : base(message) { }

        public MessageUnparserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
