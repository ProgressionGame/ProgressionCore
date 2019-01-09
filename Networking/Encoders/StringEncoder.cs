using System;
using System.Text;
using Progression.IO.Encoders.Base;
using Progression.Util;

namespace Progression.IO.Encoders
{
    public class StringEncoder : EncoderBase<string>
    {
        public readonly Encoding Encoding;
        public static readonly Guid GuidNamespace = new Guid("DEF1D9D6-69FE-4378-9BD8-553B1722C013");
        
        
        public StringEncoder(Encoding encoding)
        {
            Encoding = encoding;
            Name = $"String {Encoding.WebName}";
            Guid = GuidUtil.Create(GuidNamespace, Encoding.WebName);
        }

        public override void Encode(string obj, PacketData data)
        {
            //int32 encoder is yet to shift pos
            var encodedLength = Encoding.GetBytes(obj, 0, obj.Length, data.Data, data.Position+4);
            Types.Int32.Encode(encodedLength, data);
            data.ShiftPosition(encodedLength);
        }

        public override int Estimate(string obj)
        {
            return Encoding.GetByteCount(obj) + 4;
        }

        public override string Decode(PacketData data)
        {
            var encodedLength = Types.Int32.Decode(data);
            //pos already shifted
            var pos = data.Position;
            data.ShiftPosition(encodedLength);
            return Encoding.GetString(data.Data, pos, encodedLength);
        }

        public override string Name { get; }
        public override Guid Guid { get; }
        public override bool FixedSize => false;
        
        public override bool Equals(object obj)
        {
            return obj != null & obj is StringEncoder & Equals((obj as StringEncoder)?.Encoding, Encoding) ;
        }
    }
}