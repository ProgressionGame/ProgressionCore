using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public class SimpleFeature<T> : ISimpleFeature<T> where T : SimpleFeature<T>
    {
        private DataIdentifier _dataIdentifier;

        public SimpleFeature(string name, StaticFeatureResolver<T> resolver)
        {
            Key = new Key(resolver.FeatureTypeKey, name);
            Resolver = resolver;
            Id = resolver.Register((T) this);
        }

        public int Id { get; }
        internal int Value;

        public DataIdentifier DataIdentifier {
            get => _dataIdentifier;
            set {
                if (Resolver.IsFrozen)
                    throw new FeatureResolverLockedException("DataOffset has only to be changed by FeatureManager");
                _dataIdentifier = value;
            }
        }

        public string Name => Key.Name;
        public Key Key { get; }
        public StaticFeatureResolver<T> Resolver { get; }
        
        
        public bool HasFeature(Tile tile)
        {
            return tile[DataIdentifier] == Value;
        }

        public void AddFeature(Tile tile)
        {
            tile[DataIdentifier] = Value;
        }

        public void RemoveFeature(Tile tile)
        {
            tile[DataIdentifier] = 0;
        }

        IFeatureResolver IFeature.Resolver => Resolver;
    }
}