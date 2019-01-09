using System;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Util;

namespace Progression.IO.Encoders {
    public class UInt16Encoder : StructEncoderBase<ushort>
    {
        public override void Encode(ushort obj, PacketData data)
        {
            Int16Converter conv = obj;
            var pos = data.Position;
            var bytes = data.Data;
            if (BitConverter.IsLittleEndian) {
                bytes[pos] = conv.Byte0;
                bytes[pos + 1] = conv.Byte1;
            } else {
                bytes[pos + 1] = conv.Byte0;
                bytes[pos] = conv.Byte1;
            }

            data.ShiftPosition(ByteSize);
        }

        public override int ByteSize => 2;


        public override ushort Decode(PacketData data)
        {
            var pos = data.Position;
            var bytes = data.Data;
            data.ShiftPosition(ByteSize);
            return BitConverter.IsLittleEndian
                ? new Int16Converter(bytes[pos], bytes[pos + 1])
                : new Int16Converter(bytes[pos + 1], bytes[pos]);
        }

        public override string Name => "UInt16";
        public override Guid Guid { get; } = new Guid("E7B449D6-D64D-4B2E-9677-96992F1A5C10");

    }
}