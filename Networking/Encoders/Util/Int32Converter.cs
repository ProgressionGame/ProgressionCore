
using System;
using System.Runtime.InteropServices;

namespace Progression.IO.Encoders.Util {
    //warning platform endianness specific
    [StructLayout(LayoutKind.Explicit)]
    struct Int32Converter
    {
        [FieldOffset(0)] public readonly int Int0;
        [FieldOffset(0)] public readonly uint UInt0;
        [FieldOffset(0)] public readonly float Float0; //no clue if this is save to do but dotnet currently misses a function for it
        [FieldOffset(0)] public readonly byte Byte0;
        [FieldOffset(1)] public readonly byte Byte1;
        [FieldOffset(2)] public readonly byte Byte2;
        [FieldOffset(3)] public readonly byte Byte3;

        public Int32Converter(int int0)
        {
            UInt0 = Byte0 = Byte1 = Byte2 = Byte3 = 0;
            Float0 = 0;
            Int0 = int0;
        }
        
        public Int32Converter(uint uint0)
        {
            Int0 = Byte0 = Byte1 = Byte2 = Byte3 = 0;
            Float0 = 0;
            UInt0 = uint0;
        }
        
        public Int32Converter(float float0)
        {
            Int0 = Byte0 = Byte1 = Byte2 = Byte3 = 0;
            UInt0 = 0;
            Float0 = float0;
        }

        public Int32Converter(byte byte0, byte byte1, byte byte2, byte byte3) : this()
        {
            Int0 = 0;
            UInt0 = 0;
            Float0 = 0;
            Byte0 = byte0;
            Byte1 = byte1;
            Byte2 = byte2;
            Byte3 = byte3;
        }

        public static implicit operator Int32(Int32Converter value)
        {
            return value.Int0;
        }

        public static implicit operator Int32Converter(int value)
        {
            return new Int32Converter(value);
        }

        public static implicit operator UInt32(Int32Converter value)
        {
            return value.UInt0;
        }

        public static implicit operator Int32Converter(uint value)
        {
            return new Int32Converter(value);
        }

        public static implicit operator Single(Int32Converter value)
        {
            return value.Float0;
        }

        public static implicit operator Int32Converter(float value)
        {
            return new Int32Converter(value);
        }
    }
}