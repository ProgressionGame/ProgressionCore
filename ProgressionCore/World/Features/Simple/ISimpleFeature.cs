using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Features.Simple
{
    public interface ISimpleFeature<T> : IFeature<T>, ISimpleFeature where T : class, ISimpleFeature<T>
    {
        new StaticFeatureResolver<T> Resolver { get; }
    }
    public interface ISimpleFeature
    {
        /// <summary>
        /// Id can start with 1 - this is because 0 may represent absense of feature
        /// </summary>
        int Id { get; }
        bool HasFeature(Tile tile);
        void AddFeature(Tile tile);
        void RemoveFeature(Tile tile);
        DataIdentifier DataIdentifier { get; set; }
        //StaticFeatureResolver<T> Resolver { get; }
    }
}