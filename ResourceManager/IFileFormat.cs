using System.Collections.Generic;
using System.Collections.ObjectModel;
using Progression.Util;

namespace Progression.Resources.Manager
{
    public interface IFileFormat<T> : IFileFormat
    {
        new IDecoder<T> Decoder { get; }
    }
    
    public interface IFileFormat : INameable
    {
        IReadOnlyCollection<string> FileExtensions{ get; }
        void AddFileExtension(string extension);
        IDecoder Decoder { get; }
    }
}