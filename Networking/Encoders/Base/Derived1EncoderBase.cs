using System;
using Progression.IO.Encoders.Special;
using Progression.Util;

namespace Progression.IO.Encoders.Base {
    public abstract class Derived1EncoderBase<TType, TBase, TRoot>:DerivedEncoderBase<TType, TRoot>
    {
        public Derived1EncoderBase(IEncoder<TBase, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder, string derivationTypeName, Guid guidNamespace )
        {
            BaseEncoder = baseEncoder;
            RootEncoder = rootEncoder;
            var name = $"{derivationTypeName}<{BaseEncoder.Name}>";
            Guid = GuidUtil.Create(guidNamespace, name);
            Name = name;
        }
        public IEncoder<TBase, TRoot> BaseEncoder { get; }
        public IEncoder<TRoot, TRoot> RootEncoder { get; }

        public override string Name { get; }
        public override Guid Guid { get; }
        
        
        protected override NullableEncoderBase<TType, TRoot> CreateNullable()
        {
            return new NullableEncoder<TType, TRoot>(this, RootEncoder);
        }

        protected override ArrayEncoderBase<TType, TRoot> CreateArray()
        {
            return new ArrayEncoder<TType, TRoot>(this, RootEncoder);
        }

        public override bool FixedSize => false;
    }
}