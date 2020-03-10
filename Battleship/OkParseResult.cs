using Battleship.Messages;
using System;

namespace Battleship
{
    public class OkParseResult : ParseResult
    {
        public OkParseResult(IMessage message, SequencePosition finalPosition)
        {
            Message = message;
            FinalPosition = finalPosition;
        }

        public IMessage Message { get; }
        public SequencePosition FinalPosition { get; }
    }
}
