using System.Collections.Specialized;
using Progression.Engine.Core.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public interface IStaticFeature<T> : IFeature<T>, IStaticFeature where T : class, IStaticFeature<T>
    {
        new StaticFeatureResolver<T> Resolver { get; }
    }
    public interface IStaticFeature
    {
        /// <summary>
        /// Id can start with 1 - this is because 0 may represent absense of feature
        /// </summary>
        int Id { get; }
        DataIdentifier DataIdentifier { get; set; }
        Key Key { get; }
        //StaticFeatureResolver<T> Resolver { get; }
    }
}