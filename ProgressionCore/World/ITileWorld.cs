using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.World {
    public interface ITileWorld {
        int Height { get; }
        int Width { get; }
        bool WrapVertical { get; }
        bool WrapHorizontal { get; }
        WorldMode Mode { get; }
        
        
        Tile OutOfBounds{ get; }
        
        
        byte WorldType{ get; }
        IWorldHolder Holder{ get; }
        CivilizationManager CivilizationManager{ get; }
        Tile this[ushort x, ushort y] { get; }
        Tile this[int x, int y] { get; }
        Tile this[Coordinate coord] { get; }
        Tile GetTile(ushort x, ushort y);
        Tile GetTile(int x, int y);
        Tile GetTile(Coordinate coord);
        

        Coordinate WrapCoordinate(Coordinate coord);
        
        
        int this[Coordinate c, DataIdentifier identifier] { get; set; }
        void ScheduleUpdate(WorldUpdateBase update);
    }
}