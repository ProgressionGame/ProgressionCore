using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;

namespace Progression.Engine.Core.World.Threading
{
    public class SimpleFeatureUpdate<TSimpleFeature> : WorldUpdateBase
            where TSimpleFeature : class, ISimpleFeature<TSimpleFeature>
    {
        public SimpleFeatureUpdate(Coordinate coordinate, TSimpleFeature feature, bool set)
        {
            Feature = feature;
            Coordinate = coordinate;
            Set = set;
        }
        
        public Coordinate Coordinate { get; }
        public TSimpleFeature Feature { get; }
        public bool Set { get; }
        
        public override void Execute(WorldInterface worldInterface)
        {
            worldInterface.GenericFeatureUpdate(Coordinate, Feature, Set);
        }
    }
}