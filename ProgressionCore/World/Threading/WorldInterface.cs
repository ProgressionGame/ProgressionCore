using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Util.Threading;
using Progression.Engine.Core.Civilization;

namespace Progression.Engine.Core.World.Threading
{
    public abstract class WorldInterface : ThreadingInterface<WorldInterface, WorldUpdateBase>
    {
        protected WorldInterface(TileWorld world)
        {
            World = world;
        }
        
        public TileWorld World { get; }

        private void assertFeatureWorld(FeatureWorld other)
        {
            if (!World.FeatureWorld.Equals(other))
                throw new ArgumentException(
                    "FeatureWorld not matching! This is a serious exception. This should never ever happen as FeatureWorld describes the features of a world. Setting different features will lead to undefined behavior");
        }

        public virtual void GenericFeatureUpdate<TSimpleFeature>(Coordinate coordinate, TSimpleFeature feature, bool set) where TSimpleFeature : class, ISimpleFeature<TSimpleFeature>
        {
            assertFeatureWorld(feature.Resolver.FeatureWorld);
            var tile = World[coordinate];
            if (set) {
                feature.AddFeature(tile, true);
            } else {
                
                feature.RemoveFeature(tile, true);
            }
        }

        public virtual void CivilisationOwnershipUpdate(Coordinate coordinate, Civilization.Civilization civ)
        {
            assertFeatureWorld(civ.Register.Resolver.FeatureWorld);
            var tile = World[coordinate];
            civ.Register.Resolver.SetOwner(tile, civ, true);
        }
        public virtual void CivilisationVisionUpdate(Coordinate coordinate, Civilization.Civilization civ, Vision vision)
        {
            assertFeatureWorld(civ.Register.Resolver.FeatureWorld);
            var tile = World[coordinate];
            civ.Register.Resolver.SetVision(tile, civ, vision, true);
        }
    }
}