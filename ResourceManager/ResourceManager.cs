using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Progression.Resource.Util;

namespace Progression.Resources.Manager
{
    public class ResourceManager : IResourceManager
    {
        private readonly Dictionary<string, ResourceType> _resourceTypes = new Dictionary<string, ResourceType>();
        private readonly Dictionary<ResourceTypeEnum, ResourceType> _resourceTypesEnum = new Dictionary<ResourceTypeEnum, ResourceType>();
        private readonly Dictionary<IResourceable, List<IResourceHook>> _resourceHooksWaiting = new Dictionary<IResourceable, List<IResourceHook>>();
        
        public ResourceManager(ResourceDomain environment)
        {
            Environment = environment;
        }
        
        public ResourceDomain Environment { get; private set; }
        public bool Loaded { get; private set; }


        public void Register()
        {
            
        }

        public void AddHook(IResourceable resourceable, IResourceHook hook)
        {
            if (resourceable.IsFrozen) {
                var e = resourceable.GetResourceables();
                while (e.Current != null) {
                    hook.OnHook(e.Current);
                    e.MoveNext();
                }
            } else {
                List<IResourceHook> hooks;
                if (!_resourceHooksWaiting.ContainsKey(resourceable)) {
                    hooks = new List<IResourceHook>();
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
                    hook.OnHook(e.Current);
                }
                e.MoveNext();
            }
        }
    }
}