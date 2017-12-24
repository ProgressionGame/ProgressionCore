using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Resource
{
    public delegate void ResourceHook<T> (T item) where T: IKeyed, INameable;
}