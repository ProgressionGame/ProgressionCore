using System;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Special;

namespace Progression.IO.Encoders
{
    public class BoolEncoder : StructEncoderBase<bool>

    {
        public override void Encode(bool obj, PacketData data)
        {
            data.Data[data.Position] = obj ? (byte)1 : (byte)0;
            data.ShiftPosition(1);
        }

        public override bool Decode(PacketData data)
        {
            var result = data.Data[data.Position] != 0;
            data.ShiftPosition(1);
            return result;
        }

        
        public override string Name => "Bool";
        public override Guid Guid { get; } = new Guid("73F87080-2C2E-4470-AD91-96C89C322D1F");
        public override int ByteSize => 1;

        protected override ArrayEncoderBase<bool, bool> CreateArray()
        {
            return new BoolArrayEncoder(this);
        }
    }
}