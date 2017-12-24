using System.Collections.Generic;
using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Resource
{
    /// <summary>
    /// Indicated that a IResourceable is no longer modifable. Hooks on this IResourceable are getting called and then discarded
    /// 
    /// As soon as the implementing class is frozen FreezeResourceable(IResourceable) from the resource manager is to be called. If the class is never frozen it needs to always return true for frozen
    /// </summary>
    public interface IResourceable<T> : IResourceable where T : IKeyed, INameable
    {
        new IEnumerable<T> GetResourceables();
        //new T GetResourceable(string name);
    }

    public interface IResourceable : IFrozen
    {
        IEnumerable<IKeyNameable> GetResourceables();
        //IKNamed GetResourceable(string name);
    }
}