using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public class StaticFeature<T> : IStaticFeature<T> where T : StaticFeature<T>
    {
        private DataIdentifier _dataIdentifier;

        public StaticFeature(string name, StaticFeatureResolver<T> resolver)
        {
            Key = new Key(resolver.FeatureTypeKey, name);
            Resolver = resolver;
            Id = resolver.Register((T) this);
        }

        public int Id { get; }

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
            return Resolver.IsFeatureOnTile(tile, (T) this);
        }

        public void AddFeature(Tile tile)
        {
            Resolver.AddFeature(tile, (T) this);
        }

        public void RemoveFeature(Tile tile)
        {
            Resolver.RemoveFeature(tile, (T) this);
        }

        IFeatureResolver IFeature.Resolver => Resolver;
    }
}