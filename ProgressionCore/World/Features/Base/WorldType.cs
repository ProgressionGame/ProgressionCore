using System;

namespace Progression.Engine.Core.World.Features.Base
{
    public struct WorldType
    {
        

        public WorldType(int value, byte size)
        {
            if (size > 32 || size < 1) throw new ArgumentOutOfRangeException(nameof(size), "Size must be between 1 and 32");
            Value = value;
            Size = size;
        }

        public WorldType(params bool[] values)
        {
            if (values.Length > 32) throw new ArgumentOutOfRangeException(nameof(values), "Size must be between 1 and 32");
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
            Value = value;
            Size = (byte) values.Length;
        }

        public bool this[byte index] {
            get {
                if (index > Size || index < 1) throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and " + (Size-1));
                return CheckMask(1 << index);
            }
        }

        public byte Size { get; }
        public int Value { get; }
        public bool Valid => Size > 0;

        public WorldType SetBit(byte index, bool value)
        {
            if (index > Size || index < 1) throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and " + (Size-1));
            return SetBits(1 << index, value);
        }
        
        
        public WorldType SetBits(bool set, params bool[] values) {
            if (values.Length > Size) throw new ArgumentOutOfRangeException(nameof(values), "Size must be between 1 and " + (Size-1));
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
        
        public WorldType SetBits(int mask, bool value) => new WorldType(value?Value|mask:Value&~mask, Size);
        
        
        
        public bool CheckMask(int mask) => (mask & Value) == mask;
        

        public static readonly WorldType Player = new WorldType(false, true);
        public static readonly WorldType World = new WorldType(true, true);
        public static readonly WorldType Internal = new WorldType(true, false);

        public bool Equals(WorldType other)
        {
            var tmp = 32 - Size;
            return Size == other.Size && Value<<tmp == other.Value<<tmp; //it is possible to set ignored bits with a mask. we need to ignore these bits here
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
                return (Size.GetHashCode() * 397) ^ Value<<tmp;
            }
        }
        
        //oops i accidentally implemented a bitvector like struct. my bad.
    }
}