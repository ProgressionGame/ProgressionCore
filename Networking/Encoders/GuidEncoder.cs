using System;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders
{
    public class GuidEncoder : StructEncoderBase<Guid>

    {
        public override void Encode(Guid obj, PacketData data)
        {
            var bytes = obj.ToByteArray();
            if (BitConverter.IsLittleEndian) FlipEndianness(bytes);
            System.Array.Copy(bytes, 0, data.Data, data.Position, ByteSize);
            data.ShiftPosition(ByteSize);
        }

        public override int ByteSize => 16;

        public override Guid Decode(PacketData data)
        {
            var bytes = new byte[ByteSize];
            System.Array.Copy(data.Data, data.Position, bytes, 0, ByteSize);
            if (BitConverter.IsLittleEndian) FlipEndianness(bytes);
            data.ShiftPosition(ByteSize);
            return new Guid(bytes);
        }

        private static void FlipEndianness(byte[] bytes)
        {
            var temp = bytes[3];
            bytes[3] = bytes[0];
            bytes[0] = temp;

            temp = bytes[2];
            bytes[2] = bytes[1];
            bytes[1] = temp;

            temp = bytes[5];
            bytes[5] = bytes[4];
            bytes[4] = temp;

            temp = bytes[7];
            bytes[7] = bytes[6];
            bytes[6] = temp;
        }
        
        
        public override string Name => "Guid";
        public override Guid Guid { get; } = new Guid("A5BF0DF0-D612-4DF7-8CC0-432CB1ADC606");
    }
}