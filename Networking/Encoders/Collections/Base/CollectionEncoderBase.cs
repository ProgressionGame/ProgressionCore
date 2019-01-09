using System;
using System.Collections.Generic;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Collections.Base
{
    public abstract class CollectionEncoderBase<TCollection, TBase, TRoot>:EnumerableEncoderBase<TCollection, TBase, TRoot> where TCollection : IReadOnlyCollection<TBase>
    {
        public CollectionEncoderBase(IEncoder<TBase, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder, string derivationTypeName, Guid guidNamespace) : base(baseEncoder, rootEncoder, derivationTypeName, guidNamespace) { }
    }
}