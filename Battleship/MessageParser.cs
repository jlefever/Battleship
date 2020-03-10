using System;
using System.Buffers;
using System.Text;
using Battleship.Messages;

namespace Battleship
{
    public class MessageParser
    {
        private readonly ReadOnlySequence<byte> _buffer;

        public MessageParser(ReadOnlySequence<byte> buffer)
        {
            _buffer = buffer;
            Current = buffer.Start;
        }

        public SequencePosition Current { get; private set; }

        public ParseResult ParseMessage()
        {
            if (_buffer.Length < 3)
            {
                return new IncompleteParseResult();
            }

            var id = ParseUInt16();
            ParseUInt8(); // Extension which is never used

            return id switch
            {
                0b00000 => ParseLogOn(),
                //0b00001 => ParseRejectLogOn(),
                //0b00010 => ParseAcceptLogOn(),
                //0b00011 => ParseGameType(),
                //0b00100 => ParseSubmitBoard(),
                //0b00101 => ParseRejectBoard(),
                //0b00110 => ParseAcceptBoard(),
                //0b00111 => ParseRecallBoard(),
                //0b01000 => ParseFoundGame(),
                //0b01001 => ParseRejectGame(),
                //0b01010 => ParseAcceptGame(),
                //0b01011 => ParseGameExpired(),
                //0b01100 => ParseAssignRed(),
                //0b01101 => ParseAssignBlue(),
                //0b01110 => ParseMyGuess(),
                //0b01111 => ParseTheirGuess(),
                //0b10000 => ParseBadGuess(),
                //0b10001 => ParseHit(),
                //0b10010 => ParseMiss(),
                //0b10011 => ParseSunk(),
                //0b10100 => ParseYouWin(),
                //0b10101 => ParseYouLose(),
                _ => new BadParseResult()
            };
        }

        private ParseResult ParseLogOn()
        {
            if (_buffer.Length < 36)
            {
                return new IncompleteParseResult();
            }

            var version = ParseUInt8();
            var username = ParseString(16);
            var password = ParseString(16);

            var message = new LogOnMessage(version, username, password);
            return new OkParseResult(message, Current);
        }

        private byte ParseUInt8()
        {
            return Consume(1)[0];
        }

        private ushort ParseUInt16()
        {
            return BitConverter.ToUInt16(Consume(2));
        }

        private string ParseString(int length)
        {
            return Encoding.ASCII.GetString(Consume(length)).TrimEnd('\0');
        }

        private byte[] Consume(int length)
        {
            var bytes = _buffer.Slice(Current, length).ToArray();
            Current = _buffer.GetPosition(length, Current);
            return bytes;
        }

        private static ParseResult ParseRejectLogOn()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseAcceptLogOn()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseGameType()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseSubmitBoard()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseRejectBoard()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseAcceptBoard()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseRecallBoard()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseFoundGame()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseRejectGame()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseAcceptGame()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseGameExpired()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseAssignRed()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseAssignBlue()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseMyGuess()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseTheirGuess()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseBadGuess()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseHit()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseMiss()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseSunk()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseYouWin()
        {
            throw new NotImplementedException();
        }

        private static ParseResult ParseYouLose()
        {
            throw new NotImplementedException();
        }
    }
}
