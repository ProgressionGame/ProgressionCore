using System.Collections.Generic;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Special
{
    public class NullableEncoder<TType, TRoot> : NullableEncoderBase<TType, TRoot>
    {
        public NullableEncoder(IEncoder<TType, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder) : base(baseEncoder, rootEncoder) { }
        public override void Encode(TType obj, PacketData data)
        {
            var isNull = EqualityComparer<TType>.Default.Equals(obj, default(TType));
            Types.Bool.Encode(isNull, data);
            if (!isNull) {
                BaseEncoder.Encode(obj, data);
            } 
        }

        public override TType Decode(PacketData data)
        {
            var isNull = Types.Bool.Decode(data);
            return isNull ? default(TType) : BaseEncoder.Decode(data);
        }
    }
}