using System.IO;

namespace Progression.Resource
{
    public abstract class Decoder<T> : IDecoder<T>
    {
        protected Decoder(string name)
        {
            Name = name;
        }

        public abstract T Decode(Stream stream);
        public string Name { get; }
        object IDecoder.Decode(Stream stream) => Decode(stream);
    }
}   