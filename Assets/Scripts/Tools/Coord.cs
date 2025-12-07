using System;

namespace Tools
{
    public readonly struct Coord : IEquatable<Coord>
    {
        public int Row { get; }
        public int Column { get; }

        public Coord(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Coord coord = (Coord) obj;
            return Row == coord.Row && Column == coord.Column;
        }

        public bool Equals(Coord other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override int GetHashCode()
        {
            return Row * 8 + Column;
        }

        public double Magnitude()
        {
            return Math.Sqrt(Row * Row + Column * Column);
        }

        public static bool operator ==(Coord coord1, Coord coord2)
        {
            return coord1.Equals(coord2);
        }

        public static bool operator !=(Coord coord1, Coord coord2)
        {
            return !coord1.Equals(coord2);
        }

        public static Coord operator -(Coord coord1, Coord coord2)
        {
            return new Coord(coord1.Row - coord2.Row, coord1.Column - coord2.Column);
        }
    }
}