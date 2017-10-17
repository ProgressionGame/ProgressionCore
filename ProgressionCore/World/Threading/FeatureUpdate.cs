using System.Collections.Specialized;
using Progression.Engine.Core.World.Features;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Threading
{
    public class FeatureUpdate<TFeature, TResolver> : WorldUpdateBase
            where TFeature : class, IFeature<TFeature>
            where TResolver : IFeatureResolver<TFeature>
    {
        public FeatureUpdate(Coordinate coordinate, TResolver resolver, TFeature feature, int newValue, DataIdentifier dataIdentifier)
        {
            Resolver = resolver;
            Feature = feature;
            NewValue = newValue;
            DataIdentifier = dataIdentifier;
            Coordinate = coordinate;
        }
        
        public Coordinate Coordinate { get; }
        public TResolver Resolver { get; }
        public TFeature Feature { get; }
        public int NewValue { get; }
        public DataIdentifier DataIdentifier { get; }
        
        public override void Execute(WorldInterface worldInterface)
        {
            worldInterface.GenericFeatureUpdate(Coordinate, Resolver, Feature, NewValue, DataIdentifier);
        }
    }
}