using System;

namespace Progression.Engine.Core.World.Features.Base
{
    public struct WorldType
    {
        public WorldType(int value, byte size)
        {
            if (size > 32 || size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be between 1 and 32");
            Value = value;
            Size = size;
        }

        public WorldType(params bool[] values)
        {
            if (values.Length > 32)
                throw new ArgumentOutOfRangeException(nameof(values), "Size must be between 1 and 32");
            var value = 0;
            var i = values.Length;
            while (true) {
                if (values[--i]) value |= 1;
                if (i > 0) {
                    value <<= 1;
                } else {
                    break;
                }
            }
            Value = value;
            Size = (byte) values.Length;
        }

        public bool this[byte index] {
            get {
                if (index >= Size)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and " + (Size - 1));
                return CheckMask(1 << index);
            }
        }

        public byte Size { get; }
        public int Value { get; }
        public bool Valid => Size > 0;

        public WorldType SetBit(byte index, bool value)
        {
            if (index >= Size)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and " + (Size - 1));
            return SetBits(1 << index, value);
        }


        public WorldType SetBits(bool set, params bool[] values)
        {
            if (values.Length > Size)
                throw new ArgumentOutOfRangeException(nameof(values), "Size must be between 1 and " + (Size - 1));
            var value = 0;
            var i = 0;
            while (true) {
                if (values[i]) value |= 1;
                if (++i < values.Length) {
                    value <<= 1;
                } else {
                    break;
                }
            }
            return SetBits(value, set);
        }

        public WorldType SetBits(int mask, bool value) => new WorldType(value ? Value | mask : Value & ~mask, Size);

        public static WorldType operator +(WorldType wt1, WorldType wt2)
        {
            if (wt1.Size != wt2.Size) throw new ArgumentException("Size need to match");
            return new WorldType(wt1.Value | wt2.Value, wt1.Size);
        }

        public bool CheckMask(int mask) => (mask & Value) == mask;


        public static readonly WorldType Civ = new WorldType(false, true);
        public static readonly WorldType Base = new WorldType(true, false);
        public static readonly WorldType World = Civ + Base; //new WorldType(true, true);

        public bool Equals(WorldType other)
        {
            var tmp = 32 - Size;
            //it is possible to set ignored bits with a mask. we need to ignore these bits here
            return Size == other.Size &&
                   Value << tmp == other.Value << tmp;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is WorldType && Equals((WorldType) obj);
        }

        public override int GetHashCode()
        {
            unchecked {
                var tmp = 32 - Size;
                return (Size.GetHashCode() * 397) ^ Value << tmp;
            }
        }

        //oops i accidentally implemented a bitvector like struct. my bad.
    }
}