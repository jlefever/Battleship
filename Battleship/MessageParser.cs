using Battleship.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.DataTypes;
using Battleship.Repositories;

namespace Battleship
{
    public class MessageParser
    {
        private class IncompleteException : Exception { }

        private readonly IMessageHandler _handler;
        private readonly GameTypeRepository _gameTypeRepository;
        private ReadOnlySequence<byte> _buffer;
        private SequencePosition _messageStart;

        public MessageParser(IMessageHandler handler, GameTypeRepository gameTypeRepository)
        {
            _handler = handler;
            _gameTypeRepository = gameTypeRepository;
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

            // Ignore the extension bits
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

            var gameType = new GameType(gameTypeId, boardWidth, boardHeight, ships.ToArray());
            _handler.HandleAsync(new GameTypeMessage(gameType));
            return true;
        }

        private bool ParseSubmitBoard()
        {
            var gameTypeId = ParseUInt8();
            var isValid = _gameTypeRepository.TryGet(gameTypeId, out var gameType);

            if (!isValid)
            {
                // Invalid game type
                return false;
            }

            var placements = new List<Placement>();

            // We are expecting the exact number of Positions as there are Ships in the GameType
            for (var i = 0; i < gameType.Ships.Length; i++)
            {
                var row = ParseUInt8();
                var col = ParseUInt8();
                var vertical = ParseUInt8() != 0;

                placements.Add(new Placement(new Position(row, col), vertical));
            }

            _handler.HandleAsync(new SubmitBoardMessage(gameTypeId, placements));
            return true;
        }

        private bool ParseRejectBoard()
        {
            var error = (RejectBoardErrorId)ParseUInt8();
            _handler.HandleAsync(new RejectBoardMessage(error));
            return true;
        }

        private bool ParseMyGuess()
        {
            var position = new Position(ParseUInt8(), ParseUInt8());
            _handler.HandleAsync(new MyGuessMessage(position));
            return true;
        }

        private bool ParseTheirGuess()
        {
            var position = new Position(ParseUInt8(), ParseUInt8());
            _handler.HandleAsync(new TheirGuessMessage(position));
            return true;
        }

        private bool ParseYouLose()
        {
            var position = new Position(ParseUInt8(), ParseUInt8());
            _handler.HandleAsync(new YouLoseMessage(position));
            return true;
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
