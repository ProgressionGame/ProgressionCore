using System;
using System.Text;
using Progression.IO.Encoders.Special;
using Progression.Util;

namespace Progression.IO.Encoders.Base
{
    public abstract class ArrayEncoderBase<TBase, TRoot>:Derived1EncoderBase<TBase[], TBase, TRoot>
    {
        protected ArrayEncoderBase(IEncoder<TBase, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder) : base(baseEncoder, rootEncoder, "Array", new Guid("F9251789-FEAA-4184-A9D4-9090ED902E92")) {}
        
        
        public abstract void EncodeHeadless(TBase[] objs, PacketData data);
        public abstract TBase[] DecodeHeadless(PacketData data, int arrayLength);

        public override TBase[] Decode(PacketData data)
        {
            int arrayLength = Types.Int32.Decode(data);
            return DecodeHeadless(data, arrayLength);
        }

        public override void Encode(TBase[] objs, PacketData data)
        {
            Types.Int32.Encode(objs.Length, data);
            EncodeHeadless(objs, data);
        }

        public override int Estimate(TBase[] objs)
        {
            if (BaseEncoder.FixedSize) return objs.Length * BaseEncoder.ByteSize + 4;
            
            int result = 4;
            foreach (var obj in objs) {
                result += BaseEncoder.Estimate(obj);
            }
            return result;
        }


    }
}