using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Yield
{
    // ReSharper disable once InconsistentNaming
    public class YieldModifyingSMFR<T> : MultiFeatureResolver<T>, ITileYieldModifer
        where T : class, ISimpleFeature<T>, ITileYieldModifer
    {
        private readonly TileYieldModifierPriority _tileModifierPriority;
        public YieldModifyingSMFR(WorldType worldType, Key key, TileYieldModifierPriority tileModifierPriority) : base(worldType, key)
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