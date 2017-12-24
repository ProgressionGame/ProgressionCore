using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Simple;

namespace Progression.Engine.Core.World.Features.Yield
{
    public class SimpleYieldModifyingFeature<T> : SimpleFeature<T>, ITileYieldModifer
        where T : SimpleYieldModifyingFeature<T>
    {
        private readonly IYieldModifer _modifier;

        public SimpleYieldModifyingFeature(string name, StaticFeatureResolver<T> resolver, IYieldModifer modifier,
            TileYieldModifierPriority priority) :
            base(name, resolver)
        {
            _modifier = modifier;
            Priority = priority;
        }

        public SimpleYieldModifyingFeature(string name, StaticFeatureResolver<T> resolver, YieldManager yieldManager,
            YieldModifierType type, double[] modifiers, TileYieldModifierPriority priority) :
            base(name, resolver)
        {
            switch (type) {
                case YieldModifierType.Addition:
                    _modifier = new AdditionYieldModifer(yieldManager, modifiers);
                    break;
                case YieldModifierType.Multiplication:
                    _modifier = new MultiplicationYieldModifier(yieldManager, modifiers);
                    break;
                default:
                    throw new ArgumentException("Unknown modifier type");
            }
            Priority = priority;
        }

        public double Modify(YieldType type, Tile tile, double input)
        {
            return _modifier.Modify(type, tile, input);
        }

        public TileYieldModifierPriority Priority { get; }
        bool ITileYieldModifer.IsApplicable(Tile tile)
        {
            return HasFeature(tile);
        }
    }
}