namespace Progression.Engine.Core.World
{
    public interface IGridDirection
    {
        Tile Transform(Tile tile);
        float Distance();
    }
}