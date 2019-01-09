using System;
using System.Collections.Generic;
using System.Linq;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Collections.Base
{
    public abstract class EnumerableEncoderBase<TEnumerable, TBase, TRoot>:Derived1EncoderBase<TEnumerable, TBase, TRoot> where TEnumerable : IEnumerable<TBase>
    {
        public ArrayEncoderBase<TBase, TRoot> InternalArrayEncoder { get; }

        public EnumerableEncoderBase(IEncoder<TBase, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder,
            string derivationTypeName, Guid guidNamespace) : base(baseEncoder, rootEncoder, derivationTypeName,
            guidNamespace)
        {
            InternalArrayEncoder = baseEncoder.Array;
        }
        
        public override void Encode(TEnumerable obj, PacketData data)
        {
            InternalArrayEncoder.Encode(obj.ToArray(), data);
        }

        public override TEnumerable Decode(PacketData data)
        {
            var array = InternalArrayEncoder.Decode(data);
            return createObject(array);
        }

        protected abstract TEnumerable createObject(TBase[] content);
        
        public override int Estimate(TEnumerable obj)
        {
            return InternalArrayEncoder.Estimate(obj.ToArray()); //array created two times. there might be a more elegant solution with no array conversion at all
        }
    }
}