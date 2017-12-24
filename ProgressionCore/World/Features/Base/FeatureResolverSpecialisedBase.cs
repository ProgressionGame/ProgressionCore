using System.Collections;
using System.Collections.Generic;
using Progression.Resource;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Base {
    public abstract class FeatureResolverSpecialisedBase<T> : IFeatureResolver<T> where T : class, IFeature<T> {
        protected FeatureResolverSpecialisedBase(Key featureTypeKey)
        {
            FeatureTypeKey = featureTypeKey;
            KeyFlavour = new KeyFlavour(this);
        }
        
        //abstract impl
        public abstract IEnumerator<T> GetEnumerator(); //idk whether this is a good idea or not
        public abstract void Freeze(FeatureWorld fw);
        public abstract int Count { get; }
        public abstract T Get(int index);
        public abstract DataIdentifier[] GenerateIdentifiers();
        public abstract DataIdentifier GetIdentifier(int index);
        public abstract bool IsFrozen { get; protected set; }
        
        
        //hidden abstract impl
//        protected abstract bool HasFeature(Tile tile);
//        protected abstract bool IsFeatureOnTile(Tile tile, T feature);
//        protected abstract void AddFeature(Tile tile, T feature);
//        protected abstract void RemoveFeature(Tile tile, T feature);


        //public impl
        public Key FeatureTypeKey { get; }
        public FeatureWorld FeatureWorld { get; set; }
        
        
        //hidden impl
        protected internal KeyFlavour KeyFlavour { get; }

        KeyFlavour IKeyFlavourable.KeyFlavour => KeyFlavour;
        
        
//        bool IFeatureResolver<T>.IsFeatureOnTile(Tile tile, T feature) => IsFeatureOnTile(tile, feature);
//        void IFeatureResolver<T>.AddFeature(Tile tile, T feature) => AddFeature(tile, feature);
//        void IFeatureResolver<T>.RemoveFeature(Tile tile, T feature) => RemoveFeature(tile, feature);
//        bool IFeatureResolver.HasFeature(Tile tile) => HasFeature(tile);
        
        //hidden interface duplicate 
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
//        bool IFeatureResolver.IsFeatureOnTile(Tile tile, IFeature feature) => IsFeatureOnTile(tile, (T) feature);
//        void IFeatureResolver.AddFeature(Tile tile, IFeature feature) => AddFeature(tile, (T) feature);
//        void IFeatureResolver.RemoveFeature(Tile tile, IFeature feature) => RemoveFeature(tile, (T) feature);
        IEnumerator<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumeratorWrapper<T,IKeyNameable>(GetEnumerator());
        IEnumerator<T> IResourceable<T>.GetResourceables() => GetEnumerator();
        Key IKeyed.Key => FeatureTypeKey;

    }
}