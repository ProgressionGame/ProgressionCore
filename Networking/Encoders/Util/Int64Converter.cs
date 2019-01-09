
using System;
using System.Runtime.InteropServices;

namespace Progression.IO.Encoders.Util {
    //warning platform endianness specific
    [StructLayout(LayoutKind.Explicit)]
    struct Int64Converter
    {
        [FieldOffset(0)] public readonly long Int0;
        [FieldOffset(0)] public readonly ulong UInt0;
        [FieldOffset(0)] public readonly byte Byte0;
        [FieldOffset(1)] public readonly byte Byte1;
        [FieldOffset(2)] public readonly byte Byte2;
        [FieldOffset(3)] public readonly byte Byte3;
        [FieldOffset(4)] public readonly byte Byte4;
        [FieldOffset(5)] public readonly byte Byte5;
        [FieldOffset(6)] public readonly byte Byte6;
        [FieldOffset(7)] public readonly byte Byte7;

        public Int64Converter(long int0)
        {
            UInt0 = Byte0 = Byte1 = Byte2 = Byte3 = Byte4 = Byte5 = Byte6 = Byte7 = 0;
            Int0 = int0;
        }
        
        public Int64Converter(ulong uint0)
        {
            Int0 = Byte0 = Byte1 = Byte2 = Byte3 = Byte4 = Byte5 = Byte6 = Byte7 = 0;
            UInt0 = uint0;
        }

        public Int64Converter(byte byte0, byte byte1, byte byte2, byte byte3, byte byte4, byte byte5, byte byte6, byte byte7) : this()
        {
            Int0 = 0;
            UInt0 = 0;
            Byte0 = byte0;
            Byte1 = byte1;
            Byte2 = byte2;
            Byte3 = byte3;
            Byte4 = byte4;
            Byte5 = byte5;
            Byte6 = byte6;
            Byte7 = byte7;
            
        }

        public static implicit operator Int64(Int64Converter value)
        {
            return value.Int0;
        }

        public static implicit operator Int64Converter(long value)
        {
            return new Int64Converter(value);
        }

        public static implicit operator UInt64(Int64Converter value)
        {
            return value.UInt0;
        }

        public static implicit operator Int64Converter(ulong value)
        {
            return new Int64Converter(value);
        }

        public static implicit operator Double(Int64Converter value)
        {
            return BitConverter.Int64BitsToDouble(value.Int0);
        }

        public static implicit operator Int64Converter(double value)
        {
            return new Int64Converter(BitConverter.DoubleToInt64Bits(value));
        }
    }
}