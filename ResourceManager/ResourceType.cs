using Progression.Util;

namespace Progression.Resources.Manager
{
    public class ResourceType<T> : ResourceType
    {
    }
    public class ResourceType : INameable
    {
         public string Name { get; }
    }
}