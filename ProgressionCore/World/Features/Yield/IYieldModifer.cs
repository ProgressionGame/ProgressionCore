namespace Progression.Engine.Core.World.Features.Yield
{
    public interface IYieldModifer
    {
        double Modify(YieldType type, Tile tile, double input);
    }
}