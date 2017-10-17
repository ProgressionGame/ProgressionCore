using System;

namespace Progression.Engine.Core.World.Features.Base
{
    public class FeatureResolverLockedException : InvalidOperationException
    {
        public FeatureResolverLockedException() { }

        public FeatureResolverLockedException(string message) : base(message) { }

        public FeatureResolverLockedException(string message, Exception innerException) :
            base(message, innerException) { }
    }
}