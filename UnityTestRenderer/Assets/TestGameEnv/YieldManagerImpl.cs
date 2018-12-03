using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Util.Keys;

namespace TestGameEnv
{
    public class YieldManagerImpl : YieldManager
    {
        public YieldManagerImpl(Key key) : base(key) { }
        private readonly List<ITileYieldModifer> _modifers = new List<ITileYieldModifer>();

        public override void AddTileYieldModifier(ITileYieldModifer modifer)
        {
            _modifers.Add(modifer);
        }

        public override double CalcYield(YieldType type, Tile tile)
        {
            double result = 0;
            // ReSharper disable once LoopCanBeConvertedToQuery - high frequenzy code! even tho its test code
            foreach (var modifer in _modifers) {
                if (modifer.IsApplicable(tile)) result = modifer.Modify(type, tile, result);
            }
            return result;
        }
    }
}