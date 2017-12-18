using System;
using System.Collections.Generic;
using Progression.Util;
using Progression.Util.Generics;

namespace Progression.Resources.Manager
{
    public sealed class ResourceType<T> : ResourceType
    {
        private readonly Dictionary<string, IFileFormat<T>> _fileFormats;

        public new IEnumerable<IFileFormat<T>> GetFileFormats() => _fileFormats.Values;
        public new IFileFormat<T> GetFileFormats(string extension) => _fileFormats[extension];

        public ResourceType(ResourceCategoryEnum category, string name) : this(name, category.ToOther(),
            new DoubleTypeDictionary<string, IFileFormat<T>, IFileFormat>()) { }

        public ResourceType(ResourceTypeEnum resType) : this(resType.ToString(), resType,
            new DoubleTypeDictionary<string, IFileFormat<T>, IFileFormat>())
        {
            if (!resType.IsValid()) throw new ArgumentException("Type must be valid");
            if (!resType.IsOther()) throw new ArgumentException($"{resType.ToString()} requires a name");
        }

        private ResourceType(string name, ResourceTypeEnum resType,
            DoubleTypeDictionary<string, IFileFormat<T>, IFileFormat> fileFormats) : base(name, resType, fileFormats)
        {
            _fileFormats = fileFormats;
        }
    }

    public abstract class ResourceType : INameable
    {
        private readonly IReadOnlyDictionary<string, IFileFormat> _fileFormats;

        internal ResourceType(string name, ResourceTypeEnum resType,
            IReadOnlyDictionary<string, IFileFormat> fileFormats)
        {
            Name = name;
            _fileFormats = fileFormats;
            Category = resType.Category();
            ResType = resType;
        }


        public IEnumerable<IFileFormat> GetFileFormats() => _fileFormats.Values;
        public IFileFormat GetFileFormats(string extension) => _fileFormats[extension];

        public string Name { get; }
        public ResourceCategoryEnum Category { get; }
        public ResourceTypeEnum ResType { get; }
    }
}