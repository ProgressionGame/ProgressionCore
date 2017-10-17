namespace Progression.Engine.Core.World.Features.Base
{
    public interface IMultiFeatureResolver<T> : IFeatureResolver<T>, IMultiFeatureResolver where T : class, IFeature<T>
    {
        new T[] GetFeatures(Tile tile);
    }
    public interface IMultiFeatureResolver : IFeatureResolver
    {
        IFeature[] GetFeatures(Tile tile);
    }
}