namespace Progression.Engine.Core.World.Features.Yield
{
    public interface ITileYieldModifer : IYieldModifer
    {
        TileYieldModifierPriority Priority { get; }
        bool IsApplicable(Tile tile);
    }
}