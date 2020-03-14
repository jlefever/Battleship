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
        /// <summary>
        /// This is thrown inside this class to signal down the call
        /// stack that we ran out of data. This is not an error. We will
        /// always run out of data eventually.
        /// </summary>
        private class IncompleteException : Exception { }

        /// <summary>
        /// We emit parsed messages to this handler.
        /// </summary>
        private readonly IMessageHandler _handler;

        /// <summary>
        /// A GameTypeRepository is required to correctly parse a BoardSubmit
        /// message.
        /// </summary>
        private readonly GameTypeRepository _gameTypeRepository;

        /// <summary>
        /// This is the buffer we are currently reading from.
        /// </summary>
        private ReadOnlySequence<byte> _buffer;

        /// <summary>
        /// This class is responsible for translating a byte sequence to concrete IMessages.
        /// </summary>
        /// <param name="handler">Parsed IMessages are emitted here.</param>
        /// <param name="gameTypeRepository">
        /// A repository of GameTypes received from a particular server.
        /// </param>
        public MessageParser(IMessageHandler handler, GameTypeRepository gameTypeRepository)
        {
            _handler = handler;
            _gameTypeRepository = gameTypeRepository;
        }

        /// <summary>
        /// Parses an entire buffer until it reads invalid input or runs out
        /// of data. Emits parsed IMessages to the IMessageHandler.
        /// </summary>
        /// <param name="buffer">The buffer to be read from.</param>
        /// <returns>
        /// Returns the exclusive SequencePosition where we stopped parsing.
        /// Returns null if invalid input.
        /// </returns>
        public SequencePosition? Parse(ReadOnlySequence<byte> buffer)
        {
            _buffer = buffer;

            while (true)
            {
                var start = _buffer.Start;

                try
                {
                    if (!ParseMessage()) return null;
                }
                catch (IncompleteException)
                {
                    return start;
                }
            }
        }

        /// <summary>
        /// Attempt to parse a complete message.
        /// </summary>
        /// <returns>False if there was invalid input</returns>
        private bool ParseMessage()
        {
            var id = (MessageTypeId)UInt16();

            // Move past and ignore the extension bits.
            UInt8();

            return id switch
            {
                MessageTypeId.LogOn => ParseLogOn(),
                MessageTypeId.RejectLogOn => ParseRejectLogOn(),
                MessageTypeId.AcceptLogOn => HandleBasic(id),
                MessageTypeId.GameType => ParseGameType(),
                MessageTypeId.SubmitBoard => ParseSubmitBoard(),
                MessageTypeId.RejectBoard => ParseRejectBoard(),
                MessageTypeId.AcceptBoard => HandleBasic(id),
                MessageTypeId.RecallBoard => HandleBasic(id),
                MessageTypeId.FoundGame => HandleBasic(id),
                MessageTypeId.RejectGame => HandleBasic(id),
                MessageTypeId.AcceptGame => HandleBasic(id),
                MessageTypeId.GameExpired => HandleBasic(id),
                MessageTypeId.AssignRed => HandleBasic(id),
                MessageTypeId.AssignBlue => HandleBasic(id),
                MessageTypeId.MyGuess => ParseMyGuess(),
                MessageTypeId.TheirGuess => ParseTheirGuess(),
                MessageTypeId.BadGuess => HandleBasic(id),
                MessageTypeId.Hit => HandleBasic(id),
                MessageTypeId.Miss => HandleBasic(id),
                MessageTypeId.Sunk => HandleBasic(id),
                MessageTypeId.YouWin => HandleBasic(id),
                MessageTypeId.YouLose => ParseYouLose(),
                _ => false
            };
        }

        /// <summary>
        /// Emit any one of the MessageTypes that do not have a body.
        /// </summary>
        /// <param name="id">The MessageTypeId to emit.</param>
        /// <returns>true</returns>
        private bool HandleBasic(MessageTypeId id)
        {
            _handler.Handle(new BasicMessage(id));
            return true;
        }

        private bool ParseLogOn()
        {
            var version = UInt8();
            var username = String16();
            var password = String16();
            _handler.Handle(new LogOnMessage(version, username, password));
            return true;
        }

        private bool ParseRejectLogOn()
        {
            _handler.Handle(new RejectLogOnMessage(UInt8()));
            return true;
        }

        private bool ParseGameType()
        {
            var gameTypeId = UInt8();
            var boardWidth = UInt8();
            var boardHeight = UInt8();

            var prevShip = byte.MaxValue;
            var ships = new List<byte>();

            for (var i = 0; i < 127; i++)
            {
                var ship = UInt8();

                // Ships must be listed in descending order.
                if (ship > prevShip) return false;

                // Only add ships if they are non-zero.
                if (ship != 0) ships.Add(ship);

                prevShip = ship;
            }

            // A GameType needs at least one ship.
            if (!ships.Any()) return false;

            var gameType = new GameType(gameTypeId, boardWidth, boardHeight, ships.ToArray());
            _handler.Handle(new GameTypeMessage(gameType));
            return true;
        }

        private bool ParseSubmitBoard()
        {
            var gameTypeId = UInt8();
            var isValid = _gameTypeRepository.TryGet(gameTypeId, out var gameType);

            // Confirm this is a valid game type.
            if (!isValid) return false;

            var placements = new List<Placement>();

            // We are expecting the exact number of Positions as there are Ships in the GameType.
            for (var i = 0; i < gameType.Ships.Length; i++)
            {
                var row = UInt8();
                var col = UInt8();
                var vertical = UInt8() != 0;

                placements.Add(new Placement(new Position(row, col), vertical));
            }

            _handler.Handle(new SubmitBoardMessage(gameTypeId, placements));
            return true;
        }

        private bool ParseRejectBoard()
        {
            var error = (RejectBoardErrorId)UInt8();
            _handler.Handle(new RejectBoardMessage(error));
            return true;
        }

        private bool ParseMyGuess()
        {
            var position = new Position(UInt8(), UInt8());
            _handler.Handle(new MyGuessMessage(position));
            return true;
        }

        private bool ParseTheirGuess()
        {
            var position = new Position(UInt8(), UInt8());
            _handler.Handle(new TheirGuessMessage(position));
            return true;
        }

        private bool ParseYouLose()
        {
            var position = new Position(UInt8(), UInt8());
            _handler.Handle(new YouLoseMessage(position));
            return true;
        }

        /// <summary>
        /// Consume an unsigned 8-bit integer.
        /// </summary>
        /// <returns></returns>
        private byte UInt8()
        {
            return Consume(1)[0];
        }

        /// <summary>
        /// Consume an unsigned 16-bit integer.
        /// </summary>
        /// <returns>The value read</returns>
        private ushort UInt16()
        {
            return BitConverter.ToUInt16(Consume(2));
        }

        /// <summary>
        /// Consume a 16 byte long string.
        /// </summary>
        /// <returns>The value read</returns>
        private string String16()
        {
            return Encoding.ASCII.GetString(Consume(16)).TrimEnd('\0');
        }

        /// <summary>
        /// Moves the cursor forward length bytes returning read data.
        /// </summary>
        /// <param name="length">How far to read.</param>
        /// <returns>Read data as byte[]</returns>
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
