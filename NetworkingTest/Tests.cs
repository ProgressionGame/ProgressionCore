using System;
using System.Diagnostics;
using Progression.IO;
using Progression.IO.Encoders.Base;
using Progression.IO.Encoders.Special;
using Xunit;

namespace NetworkingTest
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            TestEncoderStruct(Types.Bool, true, false);
            TestEncoderStruct(Types.Bool, false, true);
            TestEncoderStruct(Types.Int8, (sbyte)0 ,(sbyte)1 );
            TestEncoderStruct(Types.Int8, (sbyte)127,(sbyte)1);
            TestEncoderStruct(Types.UInt8, (byte)0,(byte)1);
            TestEncoderStruct(Types.UInt8, (byte)255,(byte)1);
            TestEncoderStruct(Types.Int16, (sbyte)0,(sbyte)1);
            TestEncoderStruct(Types.Int16, (sbyte)127,(sbyte)1);
            TestEncoderStruct(Types.UInt16, (byte)0,(byte)1);
            TestEncoderStruct(Types.UInt16, (byte)255,(byte)1);
            TestEncoderStruct(Types.Int32, 0,1);
            TestEncoderStruct(Types.Int32, 127,1);
            TestEncoderStruct(Types.UInt32, (byte)0,(byte)1);
            TestEncoderStruct(Types.UInt32, (byte)255,(byte)1);
            TestEncoderStruct(Types.Int64, 0,1);
            TestEncoderStruct(Types.Int64, 127,1);
            TestEncoderStruct(Types.UInt64, (byte)0,(byte)1);
            TestEncoderStruct(Types.UInt64, (byte)255,(byte)1);
            TestEncoderStruct(Types.Float32, 0,1);
            TestEncoderStruct(Types.Float32, 127,1);
            TestEncoderStruct(Types.Float64, 0,1);
            TestEncoderStruct(Types.Float64, 255,1);
            TestEncoderStruct(Types.Guid, Guid.NewGuid(), Guid.NewGuid());
            TestEncoderClass(Types.StringUTF8, "Hello World. 世界はおはようございますー！", "");
            TestEncoderClass(Types.StringUTF16, "Hello World. 世界はおはようございますー！", "");
            TestEncoderClass(Types.StringUTF32, "Hello World. 世界はおはようございますー！", "");
            TestEncoderClass(Types.StringASCII, "Hello World.", "");
            TestEncoderStruct(new Tuple2Encoder<int, int,string, string>(Types.Int32, Types.StringASCII), (1, "2"), (3, "4"), false);
        }

        public void TestEncoderStruct<TType>(IEncoder<TType> encoder, TType in1 , TType in2, bool testnull=true) where TType : struct
        {
            TestEncoderBase<TType, TType?>(encoder, in1, in2, testnull);
        }
        public void TestEncoderClass<TType>(IEncoder<TType> encoder, TType in1 , TType in2) where TType : class
        {
            TestEncoderBase<TType, TType>(encoder, in1, in2, true);
        }

        public void TestEncoderBase<TType, TArray>(IEncoder<TType> encoder, TType in1 , TType in2, bool testnull)
        {
            TType[] arrayIn1 = new[] {in1, in2};
            TArray[] arrayIn2 = new TArray[] {(TArray)(object)in1, (TArray)(object)null, (TArray)(object)in2};
            
            
            PacketData data = new PacketData();
            data.AddEstimate(encoder.Estimate(in1));
            data.AddEstimate(encoder.Array.Estimate(arrayIn1));
            if (testnull) {
                data.AddEstimate(encoder.Nullable.Estimate(null));
                data.AddEstimate(encoder.Nullable.Estimate(in1));
                data.AddEstimate(encoder.Nullable.Array.Estimate(arrayIn2));
            }
            data.InitArray();
            encoder.Encode(in1, data);
            encoder.Array.Encode(arrayIn1, data);
            if (testnull) {
                encoder.Nullable.Encode(null, data);
                encoder.Nullable.Encode(in1, data);
                encoder.Nullable.Array.Encode(arrayIn2, data);
            }
            data.Flip();
            
            TType out1 = encoder.Decode(data);
            TType[] arrayOut1 = (TType[]) encoder.Array.Decode(data);
            if (testnull) {
                TArray outNull = (TArray) encoder.Nullable.Decode(data);
                TArray out1Null = (TArray) encoder.Nullable.Decode(data);
                TArray[] arrayOut2 = (TArray[]) encoder.Nullable.Array.Decode(data);   
            }
            
            Assert.True(Equals(in1, out1), $"Not consistent {in1} {out1}");
            
            
        }
    }
}