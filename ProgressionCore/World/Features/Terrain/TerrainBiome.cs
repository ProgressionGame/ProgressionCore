using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Engine.Core.World.Features.Yield;

namespace Progression.Engine.Core.World.Features.Terrain
{
    public class TerrainBiome : TerrainFeature<TerrainBiome>
    {
        public TerrainBiome(string name, StaticFeatureResolver<TerrainBiome> resolver, IYieldModifer modifier) : base(name, resolver, modifier) { }
        public TerrainBiome(string name, StaticFeatureResolver<TerrainBiome> resolver, YieldManager yieldManager, YieldModifierType type, double[] modifiers) : base(name, resolver, yieldManager, type, modifiers) { }
    }
}