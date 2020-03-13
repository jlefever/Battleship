using System;

namespace Battleship.DataTypes
{
    /// <summary>
    /// Represents the position and orientation of a ship.
    /// </summary>
    public struct Placement : IEquatable<Placement>
    {
        public Placement(Position position, bool vertical)
        {
            Position = position;
            Vertical = vertical;
        }

        public Position Position { get; }
        public bool Vertical { get; }

        public bool Equals(Placement other)
        {
            return Position.Equals(other.Position) && Vertical == other.Vertical;
        }

        public override bool Equals(object obj)
        {
            return obj is Placement other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Vertical);
        }

        public static bool operator ==(Placement left, Placement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Placement left, Placement right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{nameof(Position)}: {Position}, {nameof(Vertical)}: {Vertical}";
        }
    }
}
