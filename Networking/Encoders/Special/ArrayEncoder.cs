using System;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Special
{
    public class ArrayEncoder<TBase, TRoot>:ArrayEncoderBase<TBase, TRoot>
    {
        public ArrayEncoder(IEncoder<TBase, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder) : base(baseEncoder, rootEncoder) { }
        public override void EncodeHeadless(TBase[] objs, PacketData data)
        {
            foreach (var obj in objs) {
                BaseEncoder.Encode(obj, data);
            }
        }

        public override TBase[] DecodeHeadless(PacketData data, int arrayLength)
        {            
            var result = new TBase[arrayLength];
            for (int i = 0; i < arrayLength; i++) {
                result[i] = BaseEncoder.Decode(data);
            }
            return result;
        }
    }
}