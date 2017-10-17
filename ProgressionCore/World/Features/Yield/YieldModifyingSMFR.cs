using System.Linq;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Features.Yield
{
    public class YieldModifyingSMFR<T> : StaticMultiFeatureResolver<T>, ITileYieldModifer
        where T : class, IStaticFeature<T>, ITileYieldModifer
    {
        private readonly TileYieldModifierPriority _tileModifierPriority;
        public YieldModifyingSMFR(WorldType worldType, Key featureTypeKey, TileYieldModifierPriority tileModifierPriority) : base(worldType, featureTypeKey)
        {
            _tileModifierPriority = tileModifierPriority;
        }
        
        
        double IYieldModifer.Modify(YieldType type, Tile tile, double input)
        {
            var result = input;
            // ReSharper disable once LoopCanBeConvertedToQuery - high frequenzy code!
            foreach (var feature in GetFeatures(tile))
                result = feature.Modify(type, tile, result);
            return result;
        }

        TileYieldModifierPriority ITileYieldModifer.Priority => _tileModifierPriority;

        bool ITileYieldModifer.IsApplicable(Tile tile)
        {
            return HasFeature(tile);
        }
    }
}