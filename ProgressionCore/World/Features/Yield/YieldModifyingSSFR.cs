using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Yield
{
    // ReSharper disable once InconsistentNaming
    public class YieldModifyingSSFR<T> : SingleFeatureResolver<T>, ITileYieldModifer
        where T : class, ISimpleFeature<T>, ITileYieldModifer
    {
        private readonly TileYieldModifierPriority _tileModifierPriority;

        public YieldModifyingSSFR(WorldType worldType, Key key, bool optional, TileYieldModifierPriority tileModifierPriority) : base(worldType, key, optional)
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