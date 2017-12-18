using System.Collections;
using System.Collections.Generic;
using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Resource.Util
{
    /// <summary>
    /// As soon as the implementing class is frozen FreezeResourceable(IResourceable) from the resource manager is to be called. If the class is never frozen it needs to always return true for frozen
    /// </summary>
    public interface IResourceable<T> : IResourceable where T : IKeyNameable
    {
        new IEnumerator<T> GetResourceables();
        //new T GetResourceable(string name);
    }

    public interface IResourceable : IFrozen, IKeyFlavourable
    {
        IEnumerator<IKeyNameable> GetResourceables();
        //IKNamed GetResourceable(string name);
    }
}