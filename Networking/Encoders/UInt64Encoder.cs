using System;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Util;

namespace Progression.IO.Encoders {
    public class UInt64Encoder : StructEncoderBase<ulong>
    {
        public override void Encode(ulong obj, PacketData data)
        {
            Int64Converter conv = obj;
            var pos = data.Position;
            var bytes = data.Data;
            if (BitConverter.IsLittleEndian) {
                bytes[pos] = conv.Byte0;
                bytes[pos + 1] = conv.Byte1;
                bytes[pos + 2] = conv.Byte2;
                bytes[pos + 3] = conv.Byte3;
                bytes[pos + 4] = conv.Byte4;
                bytes[pos + 5] = conv.Byte5;
                bytes[pos + 6] = conv.Byte6;
                bytes[pos + 7] = conv.Byte7;
            } else {
                bytes[pos + 7] = conv.Byte0;
                bytes[pos + 6] = conv.Byte1;
                bytes[pos + 5] = conv.Byte2;
                bytes[pos + 4] = conv.Byte3;
                bytes[pos + 3] = conv.Byte4;
                bytes[pos + 2] = conv.Byte5;
                bytes[pos + 1] = conv.Byte6;
                bytes[pos] = conv.Byte7;
            }

            data.ShiftPosition(ByteSize);
        }

        public override int ByteSize => 8;

        public override ulong Decode(PacketData data)
        {
            var pos = data.Position;
            var bytes = data.Data;
            data.ShiftPosition(ByteSize);
            return BitConverter.IsLittleEndian
                ? new Int64Converter(bytes[pos], bytes[pos + 1], bytes[pos + 2], bytes[pos + 3], bytes[pos + 4], bytes[pos + 5], bytes[pos + 6], bytes[pos + 7])
                : new Int64Converter(bytes[pos + 7], bytes[pos + 6], bytes[pos + 5], bytes[pos + 4], bytes[pos + 3], bytes[pos + 2], bytes[pos + 1], bytes[pos]);
        }

        public override string Name => "UInt64";
        public override Guid Guid { get; } = new Guid("5AFBB7A4-BFE5-4297-AB2F-F49921DE4C40");

    }
}