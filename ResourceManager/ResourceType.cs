using System.Collections.Generic;
using Progression.Util;
using Progression.Util.Generics;

namespace Progression.Resources.Manager
{
    public class ResourceType<T> : ResourceType
    {
        
        private readonly Dictionary<string, IFileFormat<T>> _fileFormats;
        
        public new IEnumerable<IFileFormat<T>> GetFileFormats() => _fileFormats.Values;
        public new IFileFormat<T> GetFileFormats(string extension) => _fileFormats[extension];

        public ResourceType(string name) : this(name, new DoubleTypeDictionary<string, IFileFormat<T>, IFileFormat>())  {}
        private ResourceType(string name, DoubleTypeDictionary<string, IFileFormat<T>, IFileFormat> fileFormats) : base(name, fileFormats)
        {
            _fileFormats = fileFormats;
        }
    }
    public class ResourceType : INameable
    {
        private readonly IReadOnlyDictionary<string, IFileFormat> _fileFormats;
        
        internal ResourceType(string name, IReadOnlyDictionary<string, IFileFormat> fileFormats)
        {
            Name = name;
            _fileFormats = fileFormats;
        }

        
        
        public IEnumerable<IFileFormat> GetFileFormats() => _fileFormats.Values;
        public IFileFormat GetFileFormats(string extension) => _fileFormats[extension];

        public string Name { get; }
    }
}