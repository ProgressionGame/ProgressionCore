using System;
using System.Runtime.CompilerServices;
using Progression.Util.Async;
using Progression.Util.Keys;

namespace Progression.Resource
{
    public class LoadRequest
    {
        private readonly WeakReference<IKeyed> _holder;

        public ResourceType Type { get; }

        public IKeyed Holder {
            get {
                _holder.TryGetTarget(out var holder);
                return holder;
            }
        }

        public AttachmentKey Key { get; }
        public ResourceDomain Domain { get; }
        public IResult Result { get; }
        
        public LoadRequest(ResourceType type, IKeyed holder, AttachmentKey key, ResourceDomain domain, IResult result)
        {
            Type = type;
            _holder = new WeakReference<IKeyed>(holder);
            Key = key;
            Domain = domain;
            Result = result;
        }
    }
}