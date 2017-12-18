using System.Collections;
using System.Collections.Generic;
using Progression.Resource.Util;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public abstract class FeatureResolverBase<T> : IFeatureResolver<T> where T : class, IFeature<T>
    {
        protected FeatureResolverBase(Key featureTypeKey)
        {
            FeatureTypeKey = featureTypeKey;
            KeyFlavour = new KeyFlavour(this);
        }
        
        public abstract IEnumerator<T> GetEnumerator(); //idk whether this is a good idea or not
        public abstract bool HasFeature(Tile tile);
        public abstract void Freeze(FeatureWorld fw);
        public abstract int Count { get; }
        public abstract bool IsFeatureOnTile(Tile tile, T feature);
        public abstract void AddFeature(Tile tile, T feature);
        public abstract void RemoveFeature(Tile tile, T feature);
        public abstract T Get(int index);
        public abstract DataIdentifier[] GenerateIdentifiers();
        public abstract DataIdentifier GetIdentifier(int index);


        public Key FeatureTypeKey { get; }
        public FeatureWorld FeatureWorld { get; set; }
        protected KeyFlavour KeyFlavour { get; }
        public bool IsFrozen { get; protected set; }

        KeyFlavour IKeyFlavourable.KeyFlavour => KeyFlavour;
        
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
        bool IFeatureResolver.IsFeatureOnTile(Tile tile, IFeature feature) => IsFeatureOnTile(tile, (T) feature);
        void IFeatureResolver.AddFeature(Tile tile, IFeature feature) => AddFeature(tile, (T) feature);
        void IFeatureResolver.RemoveFeature(Tile tile, IFeature feature) => RemoveFeature(tile, (T) feature);
        IEnumerator<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumeratorWrapper<T,IKeyNameable>(GetEnumerator());
        IEnumerator<T> IResourceable<T>.GetResourceables() => GetEnumerator();
        Key IKeyed.Key => FeatureTypeKey;
    }

}