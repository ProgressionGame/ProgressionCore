using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Util.Threading;

namespace Progression.Engine.Core.World.Threading
{
    public abstract class WorldInterface : ThreadingInterface<WorldInterface, WorldUpdateBase>
    {
        protected WorldInterface(TileWorld world)
        {
            World = world;
        }
        
        public TileWorld World { get; }

        public virtual Tile GenericFeatureUpdate<TSimpleFeature>(Coordinate coordinate, TSimpleFeature feature, bool set) where TSimpleFeature : class, ISimpleFeature<TSimpleFeature>
        {
#if DEBUG 
            if (!World.FeatureWorld.Equals(feature.Resolver.FeatureWorld))
                throw new ArgumentException(
                    "FeatureWorld not matching! This is a serious exception. This should never ever happen. This exception will only be thrown in Debug mode. Please fix your programm. ");
#endif
            var tile = World[coordinate];
            if (set) {
                feature.AddFeature(tile, true);
            } else {
                
                feature.AddFeature(tile, true);
            }

            return tile;
        }
    }
}