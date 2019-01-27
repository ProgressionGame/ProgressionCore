using System.Collections.Generic;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Yield
{
    public abstract class YieldManager
    {
        private readonly List<YieldType> _types = new List<YieldType>();
        public bool Locked { get; private set; }
        public Key Key { get; }
        public int Count => _types.Count;

        protected YieldManager(Key key)
        {
            Key = key;
        }
        
        public int Register(YieldType type)
        {
            if (Locked)
                throw new YieldManagerLockedException("Yield register locked. Cannot add new yield types during game.");
            _types.Add(type);
            return _types.Count-1; 
        }

        public void Lock()
        {
            Locked = true;
        }

        public abstract void AddTileYieldModifier(ITileYieldModifer modifer);
        public abstract double CalcYield(YieldType type, Tile tile);


    }
}