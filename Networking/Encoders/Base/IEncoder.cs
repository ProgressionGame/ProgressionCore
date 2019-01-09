using System;

namespace Progression.IO.Encoders.Base {
    public interface IEncoder<TType> : IEncoder {
        void Encode(TType obj, PacketData data);
        new TType Decode(PacketData data);
        int Estimate(TType obj);
    }

    public interface IEncoder<TType, TRoot> : IEncoder<TType> {
        new NullableEncoderBase<TType, TRoot> Nullable { get; }
        new ArrayEncoderBase<TType, TRoot> Array { get; }
    }

    public interface IEncoder {
        void Encode(object obj, PacketData data);
        object Decode(PacketData data);
        int Estimate(object obj);
        string Name { get; }
        Guid Guid { get; }
        bool FixedSize { get; }
        int ByteSize { get; }
        IEncoder Nullable { get; }
        IEncoder Array { get; }
        Type EncodingType { get; }
    }
}