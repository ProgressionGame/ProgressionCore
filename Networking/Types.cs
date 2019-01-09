using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Progression.IO.Encoders;
using Progression.IO.Encoders.Base;

namespace Progression.IO
{
    public static class Types
    {
        internal static readonly Collection<IEncoder> _registeredBaseTypes = new Collection<IEncoder>();
        public static readonly IReadOnlyCollection<IEncoder> RegisteredBaseTypes = _registeredBaseTypes;
        
        public static readonly StringEncoder StringUTF8 = new StringEncoder(Encoding.UTF8);
        public static readonly StringEncoder StringUTF16 = new StringEncoder(Encoding.Unicode); //UTF16
        public static readonly StringEncoder StringUTF32 = new StringEncoder(Encoding.UTF32);
        public static readonly StringEncoder StringASCII = new StringEncoder(Encoding.ASCII);
        public static readonly Int8Encoder Int8 = new Int8Encoder();
        public static readonly UInt8Encoder UInt8 = new UInt8Encoder();
        public static readonly Int16Encoder Int16 = new Int16Encoder();
        public static readonly UInt16Encoder UInt16 = new UInt16Encoder();
        public static readonly Int32Encoder Int32 = new Int32Encoder();
        public static readonly UInt32Encoder UInt32 = new UInt32Encoder();
        public static readonly Float32Encoder Float32 = new Float32Encoder();
        public static readonly Int64Encoder Int64 = new Int64Encoder();
        public static readonly UInt64Encoder UInt64 = new UInt64Encoder();
        public static readonly Float64Encoder Float64 = new Float64Encoder();
        public static readonly BoolEncoder Bool = new BoolEncoder();
        public static readonly GuidEncoder Guid = new GuidEncoder();
    }
}