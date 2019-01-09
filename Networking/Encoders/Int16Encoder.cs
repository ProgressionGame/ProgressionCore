using System;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Util;

namespace Progression.IO.Encoders {
    public class Int16Encoder : StructEncoderBase<short>
    {
        public override void Encode(short obj, PacketData data)
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

        public override short Decode(PacketData data)
        {
            var pos = data.Position;
            var bytes = data.Data;
            data.ShiftPosition(ByteSize);
            return BitConverter.IsLittleEndian
                ? new Int16Converter(bytes[pos], bytes[pos + 1])
                : new Int16Converter(bytes[pos + 1], bytes[pos]);
        }

        public override string Name => "SInt16";
        public override Guid Guid { get; } = new Guid("460A4E1C-4E12-4210-8324-754B8230CA45");

    }
}