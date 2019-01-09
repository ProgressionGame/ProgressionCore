using System;
using System.Diagnostics;
using Progression.IO.Encoders.Special;

namespace Progression.IO.Encoders.Base
{
    public abstract class StructEncoderBase<T> : EncoderBase<T> where T : struct
    {
        public StructEncoderBase()
        {
            _wrapper = new NullableStructWrapper(this);
        }

        public override bool FixedSize => true;

        public abstract override int ByteSize { get; }

        public sealed override int Estimate(T obj)
        {
            return ByteSize;
        }

        private readonly NullableStructWrapper _wrapper;

        protected override IEncoder NullableObject => Nullable;
        private NullableEncoderBase<T?, T> _nullable;
        public new NullableEncoderBase<T?, T> Nullable => _nullable = _nullable ?? CreateNullable();

        protected new NullableEncoderBase<T?, T> CreateNullable()
        {
            return new NullableEncoder<T?, T>(_wrapper, this);
        }

        private class NullableStructWrapper : IEncoder<T?, T>
        {
            private readonly StructEncoderBase<T> _encoderImplementation;
            public NullableStructWrapper(StructEncoderBase<T> encoderImplementation)
            {
                _encoderImplementation = encoderImplementation;
            }

            public void Encode(T? obj, PacketData data)
            {
                
                _encoderImplementation.Encode(obj.Value, data);
            }

            public void Encode(object obj, PacketData data)
            {
                ((IEncoder) _encoderImplementation).Encode(obj, data);
            }

            object IEncoder.Decode(PacketData data)
            {
                return ((IEncoder) _encoderImplementation).Decode(data);
            }

            public int Estimate(object obj)
            {
                return ((IEncoder) _encoderImplementation).Estimate(obj);
            }

            public T? Decode(PacketData data)
            {
                return _encoderImplementation.Decode(data);
            }

            public int Estimate(T? obj)
            {
                return _encoderImplementation.Estimate(obj.Value);
            }

            public string Name => _encoderImplementation.Name;

            public Guid Guid => _encoderImplementation.Guid;

            public bool FixedSize => _encoderImplementation.FixedSize;

            public int ByteSize => _encoderImplementation.ByteSize;
            IEncoder IEncoder.Nullable => ((IEncoder) _encoderImplementation).Nullable;

            IEncoder IEncoder.Array => ((IEncoder) _encoderImplementation).Array;
            public Type EncodingType => _encoderImplementation.EncodingType;

            public NullableEncoderBase<T?, T> Nullable => _encoderImplementation.Nullable;

            public ArrayEncoderBase<T?, T> Array => Nullable.Array;
            
            public static implicit operator StructEncoderBase<T>(NullableStructWrapper value)
            {
                return value._encoderImplementation;
            }
            
            
            public static implicit operator NullableStructWrapper(StructEncoderBase<T> value)
            {
                return value._wrapper;
            }
            
            public static bool operator ==(NullableStructWrapper obj1, StructEncoderBase<T> obj2)
            {
                if (ReferenceEquals(obj1, null)) {
                    return false;
                }
                if (ReferenceEquals(obj2, null)) {
                    return false;
                }

                return ReferenceEquals(obj1._encoderImplementation, obj2);
            }

            public static bool operator !=(NullableStructWrapper obj1, StructEncoderBase<T> obj2)
            {
                return !(obj1 == obj2);
            }

            public override int GetHashCode()
            {
                return _encoderImplementation.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) | (obj is StructEncoderBase<T> & (StructEncoderBase<T> ) obj == this);
            }
        }
    }
}