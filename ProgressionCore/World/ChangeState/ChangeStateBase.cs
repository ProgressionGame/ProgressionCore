using Progression.Engine.Core.Civilization;
using Progression.Engine.Core.World.Threading;
using Progression.Util;

namespace Progression.Engine.Core.World.ChangeState
{
    public abstract class ChangeStateBase : TileWorldBase, IChangeState
    {

        protected ChangeStateBase(ITileWorld parent)
        {
            Parent = parent;
        }
        public ITileWorld Parent { get; }
        public override int Height => Parent.Height;

        public override int Width => Parent.Width;

        public override bool WrapVertical => Parent.WrapVertical;

        public override bool WrapHorizontal => Parent.WrapHorizontal;

        public override WorldMode Mode => Parent.Mode;
        public override byte WorldType => Parent.WorldType;

        public override IWorldHolder Holder => Parent.Holder;

        public override CivilizationManager CivilizationManager => Parent.CivilizationManager;


        public override Coordinate WrapCoordinate(Coordinate coord)
        {
            return Parent.WrapCoordinate(coord);
        }

        public override void ScheduleUpdate(WorldUpdateBase update)
        {
            //do not actually schedule update as this is to be done at a later point on time. however TODO: updates still have to be stored
        }
    }
}