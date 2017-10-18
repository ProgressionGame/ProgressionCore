using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Features.Yield
{
    // ReSharper disable once InconsistentNaming
    public class YieldModifyingSSFR<T> : StaticSingleFeatureResolver<T>, ITileYieldModifer
        where T : class, IStaticFeature<T>, ITileYieldModifer
    {
        private readonly TileYieldModifierPriority _tileModifierPriority;

        public YieldModifyingSSFR(WorldType worldType, Key featureTypeKey, bool optional, TileYieldModifierPriority tileModifierPriority) : base(worldType, featureTypeKey, optional)
        {
            _tileModifierPriority = tileModifierPriority;
        }
        
        
        double IYieldModifer.Modify(YieldType type, Tile tile, double input)
        {
            return GetFeature(tile).Modify(type, tile, input);
        }

        TileYieldModifierPriority ITileYieldModifer.Priority => _tileModifierPriority;

        bool ITileYieldModifer.IsApplicable(Tile tile)
        {
            return HasFeature(tile);
        }
    }
}