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
        ReadOnlyCollection<string> FileExtensions{ get; }
        IDecoder Decoder { get; }
    }
}