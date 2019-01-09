using System;
using Progression.IO.Encoders.Special;
using Progression.Util;

namespace Progression.IO.Encoders.Base {
    public abstract class Derived2EncoderBase<TType, TBase1, TRoot1, TBase2, TRoot2>:DerivedEncoderBase<TType, TType>
    {
        public Derived2EncoderBase(IEncoder<TBase1, TRoot1> base1Encoder, IEncoder<TBase2, TRoot2> base2Encoder, string derivationTypeName, Guid guidNamespace )
        {
            Base1Encoder = base1Encoder;
            Base2Encoder = base2Encoder;
            var name = $"{derivationTypeName}<{Base1Encoder.Name}, {Base2Encoder.Name}>";
            Guid = GuidUtil.Create(guidNamespace, name);
            Name = name;
        }
        public IEncoder<TBase1, TRoot1> Base1Encoder { get; }
        public IEncoder<TBase2, TRoot2> Base2Encoder { get; }

        public override string Name { get; }
        public override Guid Guid { get; }
        
        
        protected override NullableEncoderBase<TType, TType> CreateNullable()
        {
            return new NullableEncoder<TType, TType>(this, this);
        }

        protected override ArrayEncoderBase<TType, TType> CreateArray()
        {
            return new ArrayEncoder<TType, TType>(this, this);
        }

        public override bool FixedSize => false;
    }
}