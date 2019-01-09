using System.Collections.Generic;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Special
{
    public class NullableArrayEncoder<TBase, TRoot> : ArrayEncoder<TBase, TRoot>
    {
        public NullableArrayEncoder(IEncoder<TBase, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder) : base(baseEncoder, rootEncoder) { }
        
        public override void EncodeHeadless(TBase[] objs, PacketData data)
        {
            BoolArrayEncoder.EncodeBoolArray(objs, data, index => BaseEncoder.Encode(objs[index], data));    
        }

        public override TBase[] DecodeHeadless(PacketData data, int arrayLength)
        {
            var result = new TBase[arrayLength];
            BoolArrayEncoder.DecodeBoolArray(data, arrayLength, index => result[index] = BaseEncoder.Decode(data));
            return result;
        }

        public override int Estimate(TBase[] objs)
        {
            int bytes = 0;
            foreach (var obj in objs) {
                if (!EqualityComparer<TBase>.Default.Equals(obj, default(TBase))) bytes+=BaseEncoder.Estimate(obj);
            }
            
            return bytes + BoolArrayEncoder.getBytesLength(objs.Length) + 4;
        }
    }
}