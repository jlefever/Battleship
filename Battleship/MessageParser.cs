using Battleship.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var id = (MessageTypeId)ParseUInt16();
            ParseUInt8();

            return id switch
            {
                MessageTypeId.LogOn => ParseLogOn(),
                MessageTypeId.RejectLogOn => ParseRejectLogOn(),
                MessageTypeId.AcceptLogOn => ParseBasic(id),
                MessageTypeId.GameType => ParseGameType(),
                MessageTypeId.SubmitBoard => ParseSubmitBoard(),
                MessageTypeId.RejectBoard => ParseRejectBoard(),
                MessageTypeId.AcceptBoard => ParseBasic(id),
                MessageTypeId.RecallBoard => ParseBasic(id),
                MessageTypeId.FoundGame => ParseBasic(id),
                MessageTypeId.RejectGame => ParseBasic(id),
                MessageTypeId.AcceptGame => ParseBasic(id),
                MessageTypeId.GameExpired => ParseBasic(id),
                MessageTypeId.AssignRed => ParseBasic(id),
                MessageTypeId.AssignBlue => ParseBasic(id),
                MessageTypeId.MyGuess => ParseMyGuess(),
                MessageTypeId.TheirGuess => ParseTheirGuess(),
                MessageTypeId.BadGuess => ParseBasic(id),
                MessageTypeId.Hit => ParseBasic(id),
                MessageTypeId.Miss => ParseBasic(id),
                MessageTypeId.Sunk => ParseBasic(id),
                MessageTypeId.YouWin => ParseBasic(id),
                MessageTypeId.YouLose => ParseYouLose(),
                _ => false
            };
        }

        private bool ParseBasic(MessageTypeId id)
        {
            _handler.HandleAsync(new BasicMessage(id));
            return true;
        }

        private bool ParseLogOn()
        {
            var version = ParseUInt8();
            var username = ParseString();
            var password = ParseString();
            _handler.HandleAsync(new LogOnMessage(version, username, password));
            return true;
        }

        private bool ParseRejectLogOn()
        {
            _handler.HandleAsync(new RejectLogOnMessage(ParseUInt8()));
            return true;
        }

        private bool ParseGameType()
        {
            var gameTypeId = ParseUInt8();
            var boardWidth = ParseUInt8();
            var boardHeight = ParseUInt8();

            var prevShip = byte.MaxValue;
            var ships = new List<byte>();

            for (var i = 0; i < 127; i++)
            {
                var ship = ParseUInt8();

                if (ship > prevShip)
                {
                    // Ships must be listed in descending order.
                    return false;
                }

                if (ship != 0)
                {
                    ships.Add(ship);
                }

                prevShip = ship;
            }

            if (!ships.Any())
            {
                // A GameType needs at least one ship.
                return false;
            }

            _handler.HandleAsync(new GameTypeMessage(gameTypeId, boardWidth, boardHeight, ships.ToArray()));
            return true;
        }

        private bool ParseSubmitBoard()
        {
            throw new NotImplementedException();
        }

        private bool ParseRejectBoard()
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

        private string ParseString()
        {
            return Encoding.ASCII.GetString(Consume(16)).TrimEnd('\0');
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
