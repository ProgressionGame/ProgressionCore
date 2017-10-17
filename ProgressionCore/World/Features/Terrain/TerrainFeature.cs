using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;

namespace Progression.Engine.Core.World.Features.Terrain
{
    public class TerrainFeature<T> : StaticYieldModifyingFeature<T> where T : TerrainFeature<T>
    {
        public TerrainFeature(string name, StaticFeatureResolver<T> resolver, IYieldModifer modifier) : base(name, resolver, modifier, TileYieldModifierPriority.Terrain) { }
        public TerrainFeature(string name, StaticFeatureResolver<T> resolver, YieldManager yieldManager, YieldModifierType type, double[] modifiers) : base(name, resolver, yieldManager, type, modifiers, TileYieldModifierPriority.Terrain) { }
    }
}