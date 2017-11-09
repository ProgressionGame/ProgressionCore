using System;
using Progression.Engine.Core.Threading;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Threading
{
    public abstract class WorldInterface : ThreadingInterface<WorldInterface, WorldUpdateBase>
    {
        protected WorldInterface(TileWorld world)
        {
            World = world;
        }

        public TileWorld World { get; }


        public virtual void GenericFeatureUpdate<TFeature, TResolver>(Coordinate coord, TResolver resolver, TFeature feature, int newValue, DataIdentifier dataIdentifier)
            where TFeature : class, IFeature<TFeature>
            where TResolver : IFeatureResolver<TFeature>
        {
            //this is a race condition that should never ever happen in a release. If it happens very unexpected stuff is going to happen
#if DEBUG 
            if (!World.FeatureWorld.Equals(resolver.FeatureWorld))
                throw new ArgumentException(
                    "FeatureWorld not matching! This is a serious exception. This should never ever happen. This exception will only be thrown in Debug mode. Please fix your programm. ");
#endif

            World[coord.X, coord.Y, dataIdentifier] = newValue;
        }
    }
}