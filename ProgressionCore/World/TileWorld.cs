using System.Collections.Specialized;
using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Engine.Core.World.Threading;
using Progression.Util;

namespace Progression.Engine.Core.World
{

    public class TileWorld : TileWorldBase
    {
        public override int Height { get; }
        public override int Width { get; }
        public override bool WrapVertical { get; }
        public override bool WrapHorizontal { get; }
        public override WorldMode Mode { get; }
        public override byte WorldType { get; }
        public override IWorldHolder Holder { get; }
        public override CivilizationManager CivilizationManager { get; }
        public readonly FeatureWorld FeatureWorld;
        private readonly BitVector32[,,] _worldData;
        private ScheduleUpdate _updates;



        public TileWorld(FeatureWorld features, int height, int width, IWorldHolder holder, WorldMode mode,
            CivilizationManager civilizationManager, bool wrapVertical, bool wrapHorizontal)
        {
            Height = height;
            Width = width;
            FeatureWorld = features;
            WorldType = holder.WorldType;
            Holder = holder;
            Mode = mode;
            CivilizationManager = civilizationManager;
            WrapVertical = wrapVertical;
            WrapHorizontal = wrapHorizontal;
            _worldData = new BitVector32[height, width, features.GetTileSize(WorldType)];
        }

        public override int this[Coordinate c, DataIdentifier identifier] {
            get => _worldData[c.X, c.Y, identifier[WorldType].Index][identifier[WorldType].Section];
            set => _worldData[c.X, c.Y, identifier[WorldType].Index][identifier[WorldType].Section] = value;
        }


        public double CalcYield(int x, int y, YieldType type)
        {
            return type.Manager.CalcYield(type, GetTile(x, y));
        }

        public void RegisterUpdate(ScheduleUpdate updateHandler)
        {
            if (_updates == null) {
                _updates = updateHandler;
            } else {
                _updates += updateHandler;
            }
        }

        public override void ScheduleUpdate(WorldUpdateBase update)
        {
            _updates?.Invoke(update);
        }
        
        

        public override Coordinate WrapCoordinate(Coordinate coord)
        {
            throw new System.NotImplementedException();
        }
    }

    public delegate void ScheduleUpdate(WorldUpdateBase update);
}