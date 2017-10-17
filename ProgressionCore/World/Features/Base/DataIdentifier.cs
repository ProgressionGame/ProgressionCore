namespace Progression.Engine.Core.World.Features.Base
{
    public class DataIdentifier
    {
        public DataIdentifier(IFeatureResolver resolver, IFeature feature, int index, int bits, WorldType worldTypes)
        {
            Resolver = resolver;
            Feature = feature;
            Index = index;
            Bits = bits;
            WorldTypes = worldTypes;
        }

        public DataIdentifier(IFeatureResolver resolver, int index, int bits, WorldType worldTypes) : this(resolver, null, index, bits, worldTypes) {}
        public IFeatureResolver Resolver { get; }
        public IFeature Feature { get; }
        public bool HasFeature => Feature != null;
        public int Index { get; }
        public int Bits { get; }
        internal DataLocation[] Locations;
        public WorldType WorldTypes { get; }
        
        public DataLocation this[byte index] => Locations[index];
    }
}