using System;
using System.Collections.Generic;
using System.IO;
using Progression.IO.Encoders.Base;

namespace Progression.IO.Encoders.Special {
    public class BoolArrayEncoder : ArrayEncoder<bool, bool>
    {
        public BoolArrayEncoder(IEncoder<bool, bool> boolEncoder) : base(boolEncoder, boolEncoder) { }

        public override int Estimate(bool[] objs)
        {
            return getBytesLength(objs.Length) + 4; //rounds up and adds 4
        }

        
        public override bool[] DecodeHeadless(PacketData data, int arrayLength)
        {
            var result = new bool[arrayLength];
            DecodeBoolArray(data, arrayLength, index => result[index] = true);
            return result;
        }

        public override void EncodeHeadless(bool[] objs, PacketData data)
        {
            EncodeBoolArray(objs, data, index => { }); //i hope this delegates dont have a lot of overhead but i want to reuse code and not copy and paste
        }


        public static void DecodeBoolArray(PacketData data, int arrayLength, Process setValue)
        {
            var bytesLength = getBytesLength(arrayLength); //rounds up
            int pos = data.Position;
            data.ShiftPosition(bytesLength);
            int i = 0;
            for (int index = pos; index < pos + bytesLength; index++) {
                byte current = data.Data[index];
                for (int j = 0; j < 8 & i < arrayLength; j++) {
                    if ((current&1)==1) {
                        setValue(i);
                    }
                    current <<= 1;
                    i++;
                }
            }
        }
        
        
        public delegate void Process(int arrayIndex);
        
        public static void EncodeBoolArray<T>(T[] objs, PacketData data, Process process)
        {
            var arrayLength = objs.Length;
            var bytesLength = getBytesLength(arrayLength); //rounds up 
            int pos = data.Position;
            data.ShiftPosition(bytesLength);
            int i = 0;
            for (int index = pos; index < pos + bytesLength; index++) {
                byte current = 0;
                for (int j = 0; j < 8 & i < arrayLength; j++) {
                    if (!EqualityComparer<T>.Default.Equals(objs[i], default(T))) {
                        current++;
                        process(i);
                    }
                    current <<= 1;
                    i++;
                }
                data.Data[index] = current;
            }
        }

        public static int getBytesLength(int arrayLength) => (arrayLength + 7) / 8;
    }
}