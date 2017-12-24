using System;
using System.Collections.Generic;
using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Resource
{
    public class ResourceManager : IResourceManager
    {
        private readonly Dictionary<string, ResourceType> _resourceTypes = new Dictionary<string, ResourceType>();

        private readonly Dictionary<ResourceTypeEnum, ResourceType> _resourceTypesEnum =
            new Dictionary<ResourceTypeEnum, ResourceType>();

        private readonly Dictionary<IResourceable, object> _resourceHooks = new Dictionary<IResourceable, object>();

        public ResourceManager(ResourceDomain environment)
        {
            Environment = environment;
        }

        public static ResourceManager Instance => ResMan.GetInstance<ResourceManager>();

        public ResourceDomain Environment { get; private set; }
        public bool Loaded { get; private set; }


        public void Register() { }

        public void OnNewResourceable<T>(IResourceable<T> resourceable, T item) where T : IKeyed, INameable
        {
            if (!_resourceHooks.TryGetValue(resourceable, out var hooksObj)) return;
            ((ResourceHook<T>) hooksObj)(item);
        }

        public void AddHook<T>(IResourceable<T> resourceable, ResourceHook<T> hook) where T : IKeyed, INameable
        {
            if (resourceable.IsFrozen) {
                var e = resourceable.GetResourceables();
                foreach (var item in e) {
                    hook(item);
                }
            }
            ResourceHook<T> hooks;
            if (_resourceHooks.TryGetValue(resourceable, out var hooksObj)) {
                hooks = (ResourceHook<T>) hooksObj;
                hooks += hook;
            } else {
                hooks = hook;
            }
            _resourceHooks[resourceable] = hooks;
        }

        /// <inheritdoc cref="IResourceManager.FreezeResourceable{T}"/>
        public void FreezeResourceable<T>(IResourceable<T> resourceable) where T : IKeyed, INameable
        {
            if (resourceable.IsFrozen)
                throw new ArgumentException("This message is only to be called by IResourceable");
            if (!_resourceHooks.TryGetValue(resourceable, out var hooksObj)) return;
            var hooks = (ResourceHook<T>) hooksObj;
            var e = resourceable.GetResourceables();
            foreach (var item in e) {
                hooks(item);
            }
        }
    }
}