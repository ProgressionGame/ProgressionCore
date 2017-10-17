using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;

namespace Progression.Engine.Core.World.Features.Terrain
{
    public class TerrainVegetation : TerrainFeature<TerrainVegetation>
    {
        public TerrainVegetation(string name, StaticFeatureResolver<TerrainVegetation> resolver, IYieldModifer modifier) : base(name, resolver, modifier) { }
        public TerrainVegetation(string name, StaticFeatureResolver<TerrainVegetation> resolver, YieldManager yieldManager, YieldModifierType type, double[] modifiers) : base(name, resolver, yieldManager, type, modifiers) { }
    }
}