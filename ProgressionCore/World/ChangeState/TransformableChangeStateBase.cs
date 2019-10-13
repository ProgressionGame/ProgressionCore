using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Threading;
using Progression.Util;

namespace Progression.Engine.Core.World.ChangeState
{
    public abstract class TransformableChangeStateBase : ChangeStateBase
    {
        protected TransformableChangeStateBase(ITileWorld parent) : base(parent) { }

        protected abstract Coordinate transform(Coordinate coord);
        protected abstract bool IsContained(Coordinate coord, DataIdentifier identifier);
        protected abstract bool IsChanged(Coordinate transCoord, DataIdentifier identifier);
        protected abstract int getChange(Coordinate transCoord);
        protected abstract void setChange(Coordinate t, int v);

        
        
        public override int this[Coordinate c, DataIdentifier i] {
            get {
                Coordinate t;
                return IsContained(c, i) && IsChanged(t= transform(c), i) ? getChange(t) : Parent[c, i]; }
            
            set {
                if (IsContained(c, i)) {
                    setChange(transform(c), value);
                } else throw new InvalidOperationException("Cannot break out of change container and modify underlying world");
            }
        }
    }
}