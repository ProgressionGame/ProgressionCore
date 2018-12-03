using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public class SimpleFeature<T> : ISimpleFeature<T> where T : SimpleFeature<T>
    {
        private DataIdentifier _dataIdentifier;

        public SimpleFeature(string name, StaticFeatureResolver<T> resolver)
        {
            Key = new Key(resolver.Key, name);
            
            Resolver = resolver;
            Id = resolver.Register((T) this);
        }

        public int Id { get; }
        internal int Value=0;

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
            if (!Resolver.ValidateData(tile, (T) this, true)) throw new InvalidOperationException();
            tile[DataIdentifier] = Value;
            tile.InvokeTileUpdate((T) this, true);
        }

        public void RemoveFeature(Tile tile)
        {
            if (!Resolver.ValidateData(tile, (T) this, true)) throw new InvalidOperationException();
            tile[DataIdentifier] = 0;
            tile.InvokeTileUpdate((T) this, false);
        }

        IFeatureResolver IFeature.Resolver => Resolver;
        public KeyFlavour KeyFlavour { get; }
    }
}