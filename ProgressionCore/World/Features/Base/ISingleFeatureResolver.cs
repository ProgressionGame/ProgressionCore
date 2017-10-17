namespace Progression.Engine.Core.World.Features.Base
{
    public interface ISingleFeatureResolver<T> : IFeatureResolver<T>, ISingleFeatureResolver where T : class, IFeature<T>
    {
        new T GetFeature(Tile tile);
    }
    public interface ISingleFeatureResolver : IFeatureResolver
    {
        IFeature GetFeature(Tile tile);
        
    }
}