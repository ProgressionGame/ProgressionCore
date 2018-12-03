using JetBrains.Annotations;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Threading;
using UnityEngine;

namespace Util
{
    public class WorldUpdateInterface : WorldInterface
    {
        public WorldUpdateInterface(TileWorld world, IFeatureResolver biome, IFeatureResolver landform, BetterTileMap biomeTileMap, BetterTileMap landformTileMap) : base(world)
        {
            _biomeResolver = biome;
            _landformResolver = landform;
            _biomeTileMap = biomeTileMap;
            _landformTileMap = landformTileMap;
        }
        protected override bool ThreadWaiting => false;

        private readonly IFeatureResolver _biomeResolver;
        private readonly IFeatureResolver _landformResolver;
        private readonly BetterTileMap _biomeTileMap;
        private readonly BetterTileMap _landformTileMap;
        
        protected override void Notify()
        {
            throw new System.NotImplementedException();
        }

        public override Tile GenericFeatureUpdate<TSimpleFeature>(Coordinate coordinate, TSimpleFeature feature, bool set)
        {
            Tile changed = base.GenericFeatureUpdate(coordinate, feature, set);
            if (feature.Resolver == _biomeResolver) {
                _biomeTileMap.UpdateTile(changed);
            } else if (feature.Resolver == _landformResolver) {
                _landformTileMap.UpdateTile(changed);
            }

            return changed;
        }
    }
}