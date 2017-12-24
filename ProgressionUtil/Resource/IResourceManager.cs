using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Resource
{
    public interface IResourceManager
    {
        /// <summary>
        /// This method is only to be called by the IResourceable itself. It will fail unless IsFrozen returns true
        /// </summary>
        void FreezeResourceable<T>(IResourceable<T> resourceable) where T: IKeyed, INameable;
        void OnNewResourceable<T>(IResourceable<T> resourceable, T item) where T: IKeyed, INameable;

        
        void AddHook<T>(IResourceable<T> resourceable, ResourceHook<T> hook) where T: IKeyed, INameable;
    }
}