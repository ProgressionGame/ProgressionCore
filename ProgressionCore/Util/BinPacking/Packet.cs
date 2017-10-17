using System;

namespace Progression.Engine.Core.Util.BinPacking
{
    public class Packet : IComparable<Packet>
    {
        public readonly int Size;
        public readonly object Value;
        public Packet(object value, int size)
        {
            Value = value;
            Size = size;
        }

        public int CompareTo(Packet other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Size.CompareTo(other.Size);
        }
    }
}