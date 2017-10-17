using Progression.Engine.Core.Keys;

namespace Progression.Engine.Core.World.Features.Yield
{
    public class YieldType
    {
        public YieldType(string name, YieldManager manager)
        {
            Key = new Key(manager.Key, name);
            Manager = manager;
            Index = manager.Register(this);
        }
        
        public int Index { get; }
        public string Name => Key.Name;
        public Key Key { get; }
        public YieldManager Manager { get; }
    }
}