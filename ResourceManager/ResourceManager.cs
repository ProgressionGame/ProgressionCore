using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Progression.Util;
using Progression.Util.Async;
using Progression.Util.Keys;

namespace Progression.Resource
{
    public class ResourceManager : IResourceManager
    {
        private readonly Dictionary<string, ResourceType> _resourceTypes = new Dictionary<string, ResourceType>();
        public DirectoryInfo Directory => Utils.AssetsDirectory;

        private readonly Dictionary<ResourceTypeEnum, ResourceType> _resourceTypesEnum =
            new Dictionary<ResourceTypeEnum, ResourceType>();

        private readonly Dictionary<IResourceable, object> _resourceHooks = new Dictionary<IResourceable, object>();
        private readonly ConcurrentQueue<LoadRequest> _loadRequests = new ConcurrentQueue<LoadRequest>();

        public ResourceManager(ResourceDomain environment)
        {
            Environment = environment;
        }

        public static ResourceManager Instance => GlobalResourceManager.GetInstance<ResourceManager>();

        public static void Init(ResourceDomain domain)
        {
            GlobalResourceManager.Instance = new ResourceManager(domain);
            
        }

        public ResourceDomain Environment { get; private set; }
        public bool Loaded { get; private set; }


        public void Register(ResourceType type)
        {
            _resourceTypes.Add(type.Name, type);
        }

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


        public AsyncResult<T> LoadResource<T>(ResourceType<T> type, IKeyed holder, AttachmentKey key, ResourceDomain domain)
        {
            var result = new AsyncResult<T>();
            var request = new LoadRequest(type, holder, key, domain, result);
            _loadRequests.Enqueue(request);
            return result;
        }

        private void ProcessResourceQueue()
        {
            throw new NotImplementedException();
            while (!_loadRequests.IsEmpty) {
                if (_loadRequests.TryDequeue(out var res))
                {
                    
                }
                
            }
        }

        private void LoadResourceSync(ResourceType type, IKeyed holder)
        {
            
        }

        public FileInfo ResolveFile(Key key)
        {
            
            throw new NotImplementedException();
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