using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;
using Progression.Engine.Core.World.Features.Yield;

namespace Progression.Engine.Core.World.Features.Terrain
{
    public class TerrainLandform : TerrainFeature<TerrainLandform>
    {
        public TerrainLandform(string name, StaticFeatureResolver<TerrainLandform> resolver, IYieldModifer modifier) : base(name, resolver, modifier) { }
        public TerrainLandform(string name, StaticFeatureResolver<TerrainLandform> resolver, YieldManager yieldManager, YieldModifierType type, double[] modifiers) : base(name, resolver, yieldManager, type, modifiers) { }
    }
}