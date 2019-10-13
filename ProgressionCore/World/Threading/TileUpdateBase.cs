using Progression.Util;
using Progression.Util.Threading;

namespace Progression.Engine.Core.World.Threading
{
    public abstract class TileUpdateBase : WorldUpdateBase
    {
        protected TileUpdateBase(Coordinate coordinate)
        {
            Coordinate = coordinate;
        }

        public Coordinate Coordinate { get; }
    }
}