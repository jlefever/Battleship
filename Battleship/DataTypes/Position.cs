using System;

namespace Battleship.DataTypes
{
    /// <summary>
    /// Represents a point on the board with a row and column.
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        public Position(byte row, byte col)
        {
            Row = row;
            Col = col;
        }

        public byte Row { get; }
        public byte Col { get; }

        public bool Equals(Position other)
        {
            return Row == other.Row && Col == other.Col;
        }

        public override bool Equals(object obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{nameof(Row)}: {Row}, {nameof(Col)}: {Col}";
        }
    }
}
