using System.Collections.Specialized;

namespace Progression.Engine.Core.World
{
    public struct DataLocation
    {
        public DataLocation(ushort index, BitVector32.Section section)
        {
            Index = index;
            Section = section;
        }
        
        public ushort Index { get; }
        public BitVector32.Section Section { get; }
    }
}