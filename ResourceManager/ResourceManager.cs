using System;
using System.Collections.Generic;

namespace Progression.Resource
{
    public class ResourceManager : IResourceManager
    {
        private readonly Dictionary<string, ResourceType> _resourceTypes = new Dictionary<string, ResourceType>();
        private readonly Dictionary<ResourceTypeEnum, ResourceType> _resourceTypesEnum = new Dictionary<ResourceTypeEnum, ResourceType>();
        private readonly Dictionary<IResourceable, List<ResourceHook>> _resourceHooksWaiting = new Dictionary<IResourceable, List<ResourceHook>>();
        
        public ResourceManager(ResourceDomain environment)
        {
            Environment = environment;
        }
        
        public ResourceDomain Environment { get; private set; }
        public bool Loaded { get; private set; }


        public void Register()
        {
            
        }

        public void AddHook(IResourceable resourceable, ResourceHook hook)
        {
            if (resourceable.IsFrozen) {
                var e = resourceable.GetResourceables();
                while (e.Current != null) {
                    hook(e.Current);
                    e.MoveNext();
                }
            } else {
                List<ResourceHook> hooks;
                if (!_resourceHooksWaiting.ContainsKey(resourceable)) {
                    hooks = new List<ResourceHook>();
                    _resourceHooksWaiting[resourceable] = hooks;
                } else {
                    hooks = _resourceHooksWaiting[resourceable];
                }
                hooks.Add(hook);
                
            }
        }

        /// <inheritdoc cref="IResourceManager.FreezeResourceable"/>
        public void FreezeResourceable(IResourceable resourceable)
        {
            if (!resourceable.IsFrozen) throw new ArgumentException("This message is only to be called by IResourceable");
            if (!_resourceHooksWaiting.TryGetValue(resourceable, out var hooks)) return;
            var e = resourceable.GetResourceables();
            while (e.Current != null) {
                foreach (var hook in hooks) {
                    hook(e.Current);
                }
                e.MoveNext();
            }
        }
    }
}