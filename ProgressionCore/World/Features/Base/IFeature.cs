using Progression.Engine.Core.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public interface IFeature<T> : IFeature where T : class, IFeature<T>
    {
    }
    public interface IFeature
    {
        bool HasFeature(Tile tile);
        void AddFeature(Tile tile);
        void RemoveFeature(Tile tile);
        IFeatureResolver Resolver { get; }
        string Name { get; }
        Key Key { get; }
    }
}