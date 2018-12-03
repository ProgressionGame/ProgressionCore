using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public class SimpleFeature<T> : ISimpleFeature<T> where T : SimpleFeature<T>
    {
        private DataIdentifier _dataIdentifier;
        private int _dataRepresentation;

        public SimpleFeature(string name, StaticFeatureResolver<T> resolver)
        {
            Key = new Key(resolver.Key, name);
            
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
        
        public int DataRepresentation {
            get => _dataRepresentation;
            set {
                if (Resolver.IsFrozen)
                    throw new FeatureResolverLockedException("DataOffset has only to be changed by FeatureManager");
                _dataRepresentation = value;
            }
        }

        public string Name => Key.Name;
        public Key Key { get; }
        public StaticFeatureResolver<T> Resolver { get; }
        
        
        public bool HasFeature(Tile tile)
        {
            return tile[DataIdentifier] == _dataRepresentation;
        }

        public void AddFeature(Tile tile, bool sync=false)
        {
            if (!Resolver.ValidateData(tile, (T) this, true)) throw new InvalidOperationException();
            Console.WriteLine($"{Name} value={_dataRepresentation}");
            tile[DataIdentifier] = _dataRepresentation;
            if (!sync)tile.InvokeTileUpdate((T) this, true);
        }

        public void RemoveFeature(Tile tile, bool sync=false)
        {
            if (!Resolver.ValidateData(tile, (T) this, true)) throw new InvalidOperationException();
            tile[DataIdentifier] = 0;
            if (!sync)tile.InvokeTileUpdate((T) this, false);
        }

        IFeatureResolver IFeature.Resolver => Resolver;
        public KeyFlavour KeyFlavour { get; }
    }
}