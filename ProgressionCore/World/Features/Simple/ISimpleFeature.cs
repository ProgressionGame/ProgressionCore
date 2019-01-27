using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Features.Simple
{
    public interface ISimpleFeature<T> : IFeature<T>, ISimpleFeature where T : class, ISimpleFeature<T>
    {
        new StaticFeatureResolver<T> Resolver { get; }
    }

    public interface ISimpleFeature : IFeature

    {
    /// <summary>
    /// Id can start with 1 - this is because 0 may represent absense of feature
    /// </summary>
    int Id { get; }
    int DataRepresentation { get; set; }

    bool HasFeature(Tile tile);
    void AddFeature(Tile tile, bool remoteUpdate = false);
    void RemoveFeature(Tile tile, bool remoteUpdate = false);

    DataIdentifier DataIdentifier { get; set; }
    //StaticFeatureResolver<T> Resolver { get; }
    }
}