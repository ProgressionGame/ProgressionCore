namespace Progression.Engine.Core.World
{
    public interface IWorldHolder
    {
        
        byte WorldType { get; }
    }

    public class WorldHolder : IWorldHolder
    {
        public static readonly WorldHolder BaseWorld = new WorldHolder(0);
        
        public WorldHolder(byte worldType)
        {
            WorldType = worldType;
        }

        public byte WorldType { get; }
    }
}