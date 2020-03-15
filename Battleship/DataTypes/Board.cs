using System.Collections.Generic;
using System.Linq;

namespace Battleship.DataTypes
{
    /// <summary>
    /// Represents a player's board for a given GameType.
    /// </summary>
    public class Board
    {
        private readonly byte _width;
        private readonly byte _height;
        private readonly IList<byte> _shipLengths;
        private readonly IList<byte> _shipRemainingPieces;
        private readonly int[,] _board;

        public Board(GameType gameType)
        {
            _width = gameType.BoardWidth;
            _height = gameType.BoardHeight;
            _shipLengths = gameType.ShipLengths;
            _shipRemainingPieces = new List<byte>(gameType.ShipLengths);
            _board = CreateBoard(_width, _height);

            GameTypeId = gameType.GameTypeId;
        }

        public byte GameTypeId { get; }

        /// <summary>
        /// Guess a position and update the board if a ship was hit.
        /// </summary>
        /// <param name="position">The position guessed</param>
        /// <returns>Hit, Miss, Sunk, or Win</returns>
        public GuessResult Guess(Position position)
        {
            // If they guess out of bounds that is a miss.
            if (IsOutOfBounds(position)) return GuessResult.Miss;

            var shipIndex = _board[position.Row, position.Col];

            // If the shipIndex is 0, then there was no ship there.
            if (shipIndex < 0) return GuessResult.Miss;

            // Update the board so any future guesses will miss.
            _board[position.Row, position.Col] = -2;

            // Decrease the piece count of the found ship by one.
            _shipRemainingPieces[shipIndex] = (byte) (_shipRemainingPieces[shipIndex] - 1);

            // If there are no more pieces left, it was a win.
            if (!_shipRemainingPieces.Any(remaining => remaining > 0))
            {
                return GuessResult.Win;
            }

            // If that ship has no more pieces left, return sunk.
            // Otherwise, it was a hit.
            return _shipRemainingPieces[shipIndex] == 0
                ? GuessResult.Sunk
                : GuessResult.Hit;
        }

        /// <summary>
        /// Attempt to place a ship on the board.
        /// </summary>
        /// <param name="p">The placement of the ship</param>
        /// <param name="shipIndex">The index of the ship to be placed</param>
        /// <returns></returns>
        public bool TryPlace(Placement p, int shipIndex)
        {
            if (!IsValidPlacement(p, shipIndex)) return false;

            var length = _shipLengths[shipIndex];

            if (p.Vertical)
            {
                for (var i = 0; i < length; i++)
                {
                    _board[p.Position.Row + i, p.Position.Col] = shipIndex;
                }
            }
            else
            {
                for (var i = 0; i < length; i++)
                {
                    _board[p.Position.Row, p.Position.Col + i] = shipIndex;
                }
            }

            return true;
        }

        /// <summary>
        /// Is this ship in bounds and not overlapping?
        /// </summary>
        /// <param name="p">The placement of the ship</param>
        /// <param name="shipIndex">The index of the ship to be placed</param>
        /// <returns></returns>
        public bool IsValidPlacement(Placement p, int shipIndex)
        {
            return !IsOutOfBounds(p, shipIndex) && !IsOverlapping(p, shipIndex);
        }

        /// <summary>
        /// Would placing a ship here put it out of bounds?
        /// </summary>
        /// <param name="p">The placement of the ship</param>
        /// <param name="shipIndex">The index of the ship to be placed</param>
        /// <returns></returns>
        public bool IsOutOfBounds(Placement p, int shipIndex)
        {
            if (IsOutOfBounds(p.Position))
            {
                return true;
            }

            var length = _shipLengths[shipIndex];

            if (p.Vertical && p.Position.Row + length >= _height)
            {
                return true;
            }

            return p.Position.Col + length >= _width;
        }

        /// <summary>
        /// Is this position out of the bounds of this board?
        /// </summary>
        /// <param name="p">A position</param>
        /// <returns></returns>
        public bool IsOutOfBounds(Position p)
        {
            return p.Row >= _height || p.Col >= _width;
        }

        /// <summary>
        /// Would placing a ship here overlap it with another existing ship?
        /// </summary>
        /// <param name="p">A position</param>
        /// /// <param name="shipIndex">The index of the ship to be placed</param>
        /// <returns></returns>
        public bool IsOverlapping(Placement p, int shipIndex)
        {
            var length = _shipLengths[shipIndex];

            if (p.Vertical)
            {
                for (var i = 0; i < length; i++)
                {
                    if (_board[p.Position.Row + i, p.Position.Col] > -1)
                    {
                        // A ship is already here.
                        return true;
                    }
                }

                return false;
            }

            for (var i = 0; i < length; i++)
            {
                if (_board[p.Position.Row, p.Position.Col + i] > -1)
                {
                    // A ship is already here.
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create a board instantiated with negative numbers.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static int[,] CreateBoard(byte width, byte height)
        {
            var board = new int[width, height];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    board[i, j] = -1;
                }
            }

            return board;
        }
    }
}
