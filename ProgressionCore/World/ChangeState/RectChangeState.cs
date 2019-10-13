using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Threading;
using Progression.Util;

namespace Progression.Engine.Core.World.ChangeState
{
    public class RectChangeState : TransformableChangeStateBase
    {
        public RectChangeState(ITileWorld parent, Rect affectedRect, IFeatureResolver applicableFeatures) : base(parent)
        {
            AffectedRect = affectedRect;
            ApplicableFeatures = applicableFeatures;
            _data = new int[affectedRect.Height,affectedRect.Width];
            _changed = new bool[affectedRect.Height,affectedRect.Width];
            
        }

        public Rect AffectedRect { get; }
        public IFeatureResolver ApplicableFeatures { get; }
        private int[,] _data;
        private bool[,] _changed;


        protected override Coordinate transform(Coordinate coord)
        {
            return new Coordinate((ushort) (coord.X - AffectedRect.Base.X), (ushort) (coord.Y - AffectedRect.Base.Y));
        }

        protected override bool IsContained(Coordinate coord, DataIdentifier identifier)
        {
            return identifier.Resolver == ApplicableFeatures && AffectedRect.IsInside(coord.X, coord.Y);
        }

        protected override bool IsChanged(Coordinate t, DataIdentifier identifier)
        {
            return identifier.Resolver == ApplicableFeatures && _changed[t.X, t.Y];
        }

        protected override int getChange(Coordinate transCoord)
        {
            return _data[transCoord.X, transCoord.Y];
        }

        protected override void setChange(Coordinate t, int v)
        {
            _data[t.X, t.Y] = v;
            _changed[t.X, t.Y] = true;
        }
    }
}