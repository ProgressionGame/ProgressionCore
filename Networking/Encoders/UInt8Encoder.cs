using System;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders {
    public class UInt8Encoder : StructEncoderBase<byte>

    {
        public override void Encode(byte obj, PacketData data)
        {
            data.Data[data.Position] = obj;
            data.ShiftPosition(1);
        }

        public override byte Decode(PacketData data)
        {
            var result = data.Data[data.Position];
            data.ShiftPosition(1);
            return result;
        }

        
        public override string Name => "UInt8";
        public override Guid Guid { get; } = new Guid("734F3761-2135-42C8-ABF4-9E4C70759080");
        public override int ByteSize => 1;
    }
}