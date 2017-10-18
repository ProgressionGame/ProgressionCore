namespace Progression.Engine.Core.World.Features.Yield
{
    public class MultiplicationYieldModifier : YieldModifier
    {
        public MultiplicationYieldModifier(YieldManager manager) : base(manager, 1) { }
        public MultiplicationYieldModifier(YieldManager manager, double[] modifiers) : base(manager, modifiers) { }
        public override double Modify(YieldType type, Tile tile, double input)
        {
            return input * Modifiers[type.Index];
        }
    }
}