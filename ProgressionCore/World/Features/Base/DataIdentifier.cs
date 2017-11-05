using System;

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

        public IFeature Feature {
            get => _feature;
            set {
                if (_feature != null) throw new InvalidOperationException("Cannot change Feature");
                if (value != null && value.Resolver != Resolver) throw new ArgumentException("Cannot set feature to a feature not managed by the resolver of this DataIdenditfier");
                _feature = value;
            }
        }

        public bool HasFeature => Feature != null;
        public int Index { get; }
        public int Bits { get; }
        internal DataLocation[] Locations;
        private IFeature _feature;
        public WorldType WorldTypes { get; }
        
        public DataLocation this[byte index] => Locations[index];
    }
}