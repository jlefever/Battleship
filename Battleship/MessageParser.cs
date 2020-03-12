using System;
using System.Buffers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Battleship.Messages;

namespace Battleship
{
    public class MessageParser
    {
        private class IncompleteException : Exception { }

        private readonly IMessageHandler _handler;
        private ReadOnlySequence<byte> _buffer;
        private SequencePosition _messageStart;

        public MessageParser(IMessageHandler handler)
        {
            _handler = handler;
        }

        public SequencePosition? Parse(ReadOnlySequence<byte> buffer)
        {
            _buffer = buffer;

            while (true)
            {
                _messageStart = _buffer.Start;

                try
                {
                    if (!ParseMessage())
                    {
                        return null;
                    }
                }
                catch (IncompleteException)
                {
                    return _messageStart;
                }
            }
        }

        private bool ParseMessage()
        {
            var id = ParseUInt16();
            ParseUInt8();

            return id switch
            {
                0b00000 => ParseLogOn(),
                0b00001 => ParseRejectLogOn(),
                0b00010 => ParseAcceptLogOn(),
                0b00011 => ParseGameType(),
                0b00100 => ParseSubmitBoard(),
                0b00101 => ParseRejectBoard(),
                0b00110 => ParseAcceptBoard(),
                0b00111 => ParseRecallBoard(),
                0b01000 => ParseFoundGame(),
                0b01001 => ParseRejectGame(),
                0b01010 => ParseAcceptGame(),
                0b01011 => ParseGameExpired(),
                0b01100 => ParseAssignRed(),
                0b01101 => ParseAssignBlue(),
                0b01110 => ParseMyGuess(),
                0b01111 => ParseTheirGuess(),
                0b10000 => ParseBadGuess(),
                0b10001 => ParseHit(),
                0b10010 => ParseMiss(),
                0b10011 => ParseSunk(),
                0b10100 => ParseYouWin(),
                0b10101 => ParseYouLose(),
                _ => false
            };
        }

        private bool ParseLogOn()
        {
            var version = ParseUInt8();
            var username = ParseString(16);
            var password = ParseString(16);
            _handler.Handle(new LogOnMessage(version, username, password));
            return true;
        }

        private bool ParseRejectLogOn()
        {
            _handler.Handle(new RejectLogOnMessage(ParseUInt8()));
            return true;
        }

        private bool ParseAcceptLogOn()
        {
            throw new NotImplementedException();
        }

        private bool ParseGameType()
        {
            throw new NotImplementedException();
        }

        private bool ParseSubmitBoard()
        {
            throw new NotImplementedException();
        }

        private bool ParseRejectBoard()
        {
            throw new NotImplementedException();
        }

        private bool ParseAcceptBoard()
        {
            throw new NotImplementedException();
        }

        private bool ParseRecallBoard()
        {
            throw new NotImplementedException();
        }

        private bool ParseFoundGame()
        {
            throw new NotImplementedException();
        }

        private bool ParseRejectGame()
        {
            throw new NotImplementedException();
        }

        private bool ParseAcceptGame()
        {
            throw new NotImplementedException();
        }

        private bool ParseGameExpired()
        {
            throw new NotImplementedException();
        }

        private bool ParseAssignRed()
        {
            throw new NotImplementedException();
        }

        private bool ParseAssignBlue()
        {
            throw new NotImplementedException();
        }

        private bool ParseMyGuess()
        {
            throw new NotImplementedException();
        }

        private bool ParseTheirGuess()
        {
            throw new NotImplementedException();
        }

        private bool ParseBadGuess()
        {
            throw new NotImplementedException();
        }

        private bool ParseHit()
        {
            throw new NotImplementedException();
        }

        private bool ParseMiss()
        {
            throw new NotImplementedException();
        }

        private bool ParseSunk()
        {
            throw new NotImplementedException();
        }

        private bool ParseYouWin()
        {
            throw new NotImplementedException();
        }

        private bool ParseYouLose()
        {
            throw new NotImplementedException();
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
            try
            {
                var data = _buffer.Slice(_buffer.Start, length).ToArray();
                _buffer = _buffer.Slice(_buffer.GetPosition(length), _buffer.End);
                return data;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new IncompleteException();
            }
        }
    }
}
