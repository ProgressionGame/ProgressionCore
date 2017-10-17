using System;

namespace Progression.Engine.Core.World.Features.Base
{
    public class FeatureNotRegisteredException : InvalidOperationException
    {
        public FeatureNotRegisteredException() { }

        public FeatureNotRegisteredException(string message) : base(message) { }

        public FeatureNotRegisteredException(string message, Exception innerException) :
            base(message, innerException) { }
    }
}