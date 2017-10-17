using System;

namespace Progression.Engine.Core.World.Features.Yield
{
    public class YieldManagerLockedException : InvalidOperationException
    {
        public YieldManagerLockedException() { }

        public YieldManagerLockedException(string message) : base(message) { }

        public YieldManagerLockedException(string message, Exception innerException) :
            base(message, innerException) { }
    }
}