namespace Progression.Engine.Core.World.Features.Base
{
    public struct WorldType
    {
        private readonly bool[] _applicable;

        private WorldType(bool[] applicable)
        {
            _applicable = applicable;
        }

        public static WorldType CreateNewType(params bool[] applicable)
        {
            return new WorldType(applicable);
        }

        public bool this[byte index] => _applicable[index];
        public int Count => _applicable.Length;

        public static WorldType Player = CreateNewType(false, true);
        public static WorldType World = CreateNewType(true, true);
        public static WorldType Internal = CreateNewType(true, false);
    }
}