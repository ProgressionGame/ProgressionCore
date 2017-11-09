using System.Collections.ObjectModel;

namespace Progression.Resources.Manager
{
    public class FileFormat<T> : IFileFormat<T>
    {
        public string Name { get; }
        public ReadOnlyCollection<string> FileExtensions{ get; }
        public IDecoder<T> Decoder { get; }
        IDecoder IFileFormat.Decoder => Decoder;
    }
}