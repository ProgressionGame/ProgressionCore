namespace Progression.Engine.Core.World.Features.Yield
{
    public class AdditionYieldModifer : YieldModifier
    {
        public AdditionYieldModifer(YieldManager manager) : base(manager, 0) { }
        public AdditionYieldModifer(YieldManager manager, double[] modifiers) : base(manager, modifiers) { }
        public override double Modify(YieldType type, Tile tile, double input)
        {
            return input + Modifiers[type.Index];
        }
    }
}