namespace Progression.Engine.Core.World.Manager
{
    public interface IWMan
    {
        int WManId { get;  set; }
    }

    delegate IWMan IWManFactory(TileWorld world);
}