using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.City
{
    public class CityFeatureResolver : IFeatureResolver<CityFeature>
    {
        public CityFeatureResolver(WorldType worldType, Key key)
        {
            WorldType = worldType;
            Key = key;
            CityKey = new Key(Key, "CityFeature");
            DummyFeature = new CityFeature(this);
        }
        public CityFeature DummyFeature { get; }
        public WorldType WorldType { get; }
        public DataIdentifier DataIdentifier { get; private set; }
        
        
        public Key Key { get; }
        public Key CityKey { get; }
        
        public DataIdentifier[] GenerateIdentifiers()
        {
            DataIdentifier = new DataIdentifier(this, DummyFeature, 0, 1, WorldType);
            return new[] {DataIdentifier};
        }

        public DataIdentifier GetIdentifier(int index) => index == 0 ? DataIdentifier : throw new IndexOutOfRangeException($"{index} > 0");

        #region Hidden

        public void Freeze(FeatureWorld fw)
        {
            FeatureWorld = fw;
        }

        public int Count => 1;
        public FeatureWorld FeatureWorld { get; private set; }
        public IEnumerator<CityFeature> GetEnumerator() => new SingleItemEnumerator<CityFeature>(DummyFeature);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);

        public CityFeature Get(int index) => DummyFeature;


        IEnumerable<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumerableWrapper<CityFeature,IKeyNameable>(this);
        IEnumerable<CityFeature> IResourceable<CityFeature>.GetResourceables() => this;
        public bool IsFrozen => true;
        #endregion

        public bool HasCity(Tile tile)
        {
            return tile[DataIdentifier] == 1;
        }

        public void AddCity(City city, bool remoteUpdate = false)
        {
            
        }
    }
}