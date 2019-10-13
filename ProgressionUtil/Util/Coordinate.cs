using System;

namespace Progression.Util
{
    public struct Coordinate : IEquatable<Coordinate>
    {
        public readonly ushort X;
        public readonly ushort Y;

        public Coordinate(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
        
        #region Operators
        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o is Coordinate coordinate)
                return Equals(coordinate);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(Coordinate t)
        {
            return X==t.X&&Y==t.Y;
        }
        #endregion
    }
}