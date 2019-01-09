using System;
using System.Collections.Generic;
using System.Linq;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Special;

namespace Progression.IO.Encoders.Collections.Base {
    public abstract class DictionaryEncoderBase<TDictionary, TBase1, TRoot1, TBase2, TRoot2> : Derived2EncoderBase<TDictionary, TBase1, TRoot1, TBase2, TRoot2> where TDictionary : IReadOnlyDictionary<TBase1, TBase2>
    {
        public ArrayEncoderBase<(TBase1 key, TBase2 value), (TBase1 key, TBase2  value)> InternalArrayEncoder { get; }
        public Tuple2Encoder<TBase1, TRoot1, TBase2, TRoot2> InternalTupleEncoder{ get; }
        
        protected DictionaryEncoderBase(IEncoder<TBase1, TRoot1> base1Encoder,
            IEncoder<TBase2, TRoot2> base2Encoder, string derivationTypeName, Guid guidNamespace) : base(
            base1Encoder, base2Encoder, derivationTypeName, guidNamespace)
        {
            InternalTupleEncoder = new Tuple2Encoder<TBase1, TRoot1, TBase2, TRoot2>(base1Encoder, base2Encoder);
            InternalArrayEncoder = InternalTupleEncoder.Array;
        }
        
        public override void Encode(TDictionary obj, PacketData data)
        {
            //InternalArrayEncoder.Encode(obj.ToArray(), data);
            //todo: with current design achievable but not in a nice way
        }

        public override TDictionary Decode(PacketData data)
        {
            var array = InternalArrayEncoder.Decode(data);
            return createObject(array, data);
        }

        protected abstract TDictionary createObject((TBase1 key, TBase2 value)[] content, PacketData data);
        
        public override int Estimate(TDictionary obj)
        {
            //InternalArrayEncoder.Estimate(obj.ToArray()); //array created two times. there might be a more elegant solution with no array conversion at all
            throw new NotImplementedException();
        }
    }
}