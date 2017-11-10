using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Progression.Resources.Manager
{
    public class FileFormat<T> : IFileFormat<T>
    {
        private readonly HashSet<string> _fileExtensions;

        public FileFormat(string name, IDecoder<T> decoder, params string[] fileExtensions)
        {
            Name = name;
            Decoder = decoder;
            _fileExtensions = new HashSet<string>(fileExtensions);
        }
        public string Name { get; }

        public IReadOnlyCollection<string> FileExtensions => _fileExtensions;

        public void AddFileExtension(string extension)
        {
            _fileExtensions.Add(extension);
        }

        public IDecoder<T> Decoder { get; }
        IDecoder IFileFormat.Decoder => Decoder;
    }
}