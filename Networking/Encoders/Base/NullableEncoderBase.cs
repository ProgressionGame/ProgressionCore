using System;
using System.Collections.Generic;
using Progression.IO.Encoders.Special;

namespace Progression.IO.Encoders.Base {
    public abstract class NullableEncoderBase<TType, TRoot> : Derived1EncoderBase<TType, TType, TRoot>
    {
        protected NullableEncoderBase(IEncoder<TType, TRoot> baseEncoder, IEncoder<TRoot, TRoot> rootEncoder) : base(baseEncoder, rootEncoder, "Nullable", new Guid("F0691A4D-F47D-4FF6-901F-C8832A434ABE")) { }

        protected override NullableEncoderBase<TType, TRoot> CreateNullable()
        {
            return this;
        }

        protected override ArrayEncoderBase<TType, TRoot> CreateArray()
        {
            return new NullableArrayEncoder<TType, TRoot>(BaseEncoder, RootEncoder);
        }

        public override int Estimate(TType obj)
        {
            return EqualityComparer<TType>.Default.Equals(obj, default(TType))?1:BaseEncoder.Estimate(obj) + 1;
        }

        public override bool FixedSize => false;
    }
}