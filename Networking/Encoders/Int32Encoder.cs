using System;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Util;

namespace Progression.IO.Encoders {
    public class Int32Encoder : StructEncoderBase<int>
    {
        public override void Encode(int obj, PacketData data)
        {
            Int32Converter conv = obj;
            var pos = data.Position;
            var bytes = data.Data;
            if (BitConverter.IsLittleEndian) {
                bytes[pos] = conv.Byte0;
                bytes[pos + 1] = conv.Byte1;
                bytes[pos + 2] = conv.Byte2;
                bytes[pos + 3] = conv.Byte3;
            } else {
                bytes[pos + 3] = conv.Byte0;
                bytes[pos + 2] = conv.Byte1;
                bytes[pos + 1] = conv.Byte2;
                bytes[pos] = conv.Byte3;
            }

            data.ShiftPosition(ByteSize);
        }

        public override int ByteSize => 4;

        public override int Decode(PacketData data)
        {
            var pos = data.Position;
            var bytes = data.Data;
            data.ShiftPosition(ByteSize);
            return BitConverter.IsLittleEndian
                ? new Int32Converter(bytes[pos], bytes[pos + 1], bytes[pos + 2], bytes[pos + 3])
                : new Int32Converter(bytes[pos + 3], bytes[pos + 2], bytes[pos + 1], bytes[pos]);
        }

        public override string Name => "SInt32";
        public override Guid Guid { get; } = new Guid("7181A9C2-1B40-4557-8819-84251370709D");

    }
}