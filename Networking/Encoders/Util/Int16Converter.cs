
using System;
using System.Runtime.InteropServices;

namespace Progression.IO.Encoders.Util {
    //warning platform endianness specific
    [StructLayout(LayoutKind.Explicit)]
    struct Int16Converter
    {
        [FieldOffset(0)] public readonly short Int0;
        [FieldOffset(0)] public readonly ushort UInt0;
        [FieldOffset(0)] public readonly byte Byte0;
        [FieldOffset(1)] public readonly byte Byte1;

        public Int16Converter(short int0)
        {
            UInt0 = Byte0 = Byte1 = 0;
            Int0 = int0;
        }
        
        public Int16Converter(ushort uint0)
        {
            Int0 = Byte0 = Byte1 = 0;
            UInt0 = uint0;
        }

        public Int16Converter(byte byte0, byte byte1) : this()
        {
            Int0 = 0;
            UInt0 = 0;
            Byte0 = byte0;
            Byte1 = byte1;
        }

        public static implicit operator Int16(Int16Converter value)
        {
            return value.Int0;
        }

        public static implicit operator Int16Converter(short value)
        {
            return new Int16Converter(value);
        }

        public static implicit operator UInt16(Int16Converter value)
        {
            return value.UInt0;
        }

        public static implicit operator Int16Converter(ushort value)
        {
            return new Int16Converter(value);
        }
    }
}