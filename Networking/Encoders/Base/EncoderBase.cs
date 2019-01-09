using System;
using System.Linq;
using Progression.IO.Encoders.Special;

namespace Progression.IO.Encoders.Base
{
    public abstract class EncoderBase<TType, TRoot> : IEncoder<TType, TRoot>
    {
        protected EncoderBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            if (IsBaseEncoder) {
                if (!Types._registeredBaseTypes.Contains(this)) {
                    Types._registeredBaseTypes.Add(this);
                }
            }
        }
        public abstract void Encode(TType obj, PacketData data);
        void IEncoder.Encode(object obj, PacketData data) => Encode((TType) obj, data);

        object IEncoder.Decode(PacketData data) => Decode(data);

        int IEncoder.Estimate(object obj) => Estimate((TType) obj);

        public override bool Equals(object obj)
        {
            return obj != null & obj is IEncoder & ((IEncoder) obj)?.EncodingType == EncodingType;
        }

        public abstract TType Decode(PacketData data);
        public abstract int Estimate(TType obj);
        public abstract string Name { get; }
        public abstract Guid Guid { get; }
        public abstract bool FixedSize { get; }

        public virtual int ByteSize => -1;

        public virtual bool IsBaseEncoder => typeof(TType) == typeof(TRoot);
        
        IEncoder IEncoder.Nullable => NullableObject;
        protected virtual IEncoder NullableObject => Nullable;

        IEncoder IEncoder.Array => Array;
        public Type EncodingType => typeof(TRoot);


        private NullableEncoderBase<TType, TRoot> _nullable;
        public NullableEncoderBase<TType, TRoot> Nullable => _nullable = _nullable ?? CreateNullable();

        protected abstract NullableEncoderBase<TType, TRoot> CreateNullable();

        private ArrayEncoderBase<TType, TRoot> _array;
        public ArrayEncoderBase<TType, TRoot> Array => _array = _array ?? CreateArray();

        protected abstract ArrayEncoderBase<TType, TRoot> CreateArray();
    }
    
    public abstract class EncoderBase<T> : EncoderBase<T, T> {
        
        protected override ArrayEncoderBase<T, T> CreateArray()
        {
            return new ArrayEncoder<T, T>(this, this);
        }
        protected override NullableEncoderBase<T, T> CreateNullable()
        {
            return new NullableEncoder<T, T>(this, this);
        }
    }
}