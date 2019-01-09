using System;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Special
{
    public class Tuple2Encoder<TBase1, TRoot1, TBase2, TRoot2> : Derived2EncoderBase<(TBase1, TBase2), TBase1, TRoot1, TBase2, TRoot2>

    {
        public Tuple2Encoder(IEncoder<TBase1, TRoot1> base1Encoder, IEncoder<TBase2, TRoot2> base2Encoder) : base(base1Encoder, base2Encoder, "Tuple2", new Guid("C837157D-7629-4FD7-A701-0D9C3A15598A")) { }
        public override void Encode((TBase1, TBase2) obj, PacketData data)
        {
            Base1Encoder.Encode(obj.Item1, data);
            Base2Encoder.Encode(obj.Item2, data);
        }

        public override (TBase1, TBase2) Decode(PacketData data)
        {
            return (Base1Encoder.Decode(data), Base2Encoder.Decode(data));
        }

        public override int Estimate((TBase1, TBase2) obj)
        {
            return Base1Encoder.Estimate(obj.Item1) + Base2Encoder.Estimate(obj.Item2);
        }
        
    }
} 