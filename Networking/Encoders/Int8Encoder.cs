using System;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Special;

namespace Progression.IO.Encoders {
    public class Int8Encoder : StructEncoderBase<sbyte>

    {
        public override void Encode(sbyte obj, PacketData data)
        {
            data.Data[data.Position] = (byte)obj;
            data.ShiftPosition(1);
        }

        public override sbyte Decode(PacketData data)
        {
            var result = (sbyte)data.Data[data.Position];
            data.ShiftPosition(1);
            return result;
        }

        
        public override string Name => "SInt8";
        public override Guid Guid { get; } = new Guid("B4F02878-94D3-424A-8F2F-A120E237C30B");
        public override int ByteSize => 1;
    }
}