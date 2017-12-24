using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public interface IFeature<T> : IFeature where T : class, IFeature<T>
    {
    }
    public interface IFeature : IKeyNameable
    {
        IFeatureResolver Resolver { get; }
    }
}