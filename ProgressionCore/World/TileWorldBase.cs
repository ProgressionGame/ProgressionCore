using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.World
{
    public abstract class TileWorldBase : ITileWorld
    {
        protected TileWorldBase()
        {
            OutOfBounds = new Tile(ushort.MaxValue, ushort.MaxValue,this);
        }
        public abstract int Height { get; }
        public abstract int Width { get; }
        public abstract bool WrapVertical { get; }
        public abstract bool WrapHorizontal { get; }
        public abstract WorldMode Mode { get; }
        public Tile OutOfBounds { get; }
        public abstract byte WorldType { get; }
        public abstract IWorldHolder Holder { get; }
        public abstract CivilizationManager CivilizationManager { get; }
        public Tile this[ushort x, ushort y] => GetTile(x, y);
        public Tile this[int x, int y] => GetTile(x, y);
        public Tile this[Coordinate coord] => GetTile(coord);

        public Tile GetTile(ushort x, ushort y)
        {
            return new Tile(x, y, this);
        }

        public Tile GetTile(int x, int y)
        {
            return new Tile((ushort) x, (ushort) y, this);
        }

        public Tile GetTile(Coordinate coord)
        {
            return new Tile(coord, this);
        }

        public abstract Coordinate WrapCoordinate(Coordinate coord);
        public abstract int this[Coordinate c, DataIdentifier identifier] { get; set; }
        public abstract void ScheduleUpdate(WorldUpdateBase update);
    }
}