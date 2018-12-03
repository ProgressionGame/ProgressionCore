using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public abstract class StaticFeatureResolver<T> : IFeatureResolver<T> where T : class, ISimpleFeature<T>
    {
        protected readonly List<T> Features = new List<T>();
        protected readonly int IdOffset;
        public readonly WorldType WorldType;

        protected StaticFeatureResolver(WorldType worldType, Key key, int idOffset)
        {
            IdOffset = idOffset;
            Key = key;
            WorldType = worldType;
            key.Flavour = key.Flavour ?? new KeyFlavour(this);
            KeyFlavour = key.Flavour;
            FeatureKeyFlavour= new KeyFlavour(this);
            
        }

        public int Count => Features.Count;

        public int Register(T feature)
        {
            if (IsFrozen)
                throw new FeatureResolverLockedException("Feature locked. Cannot add new features during game.");
            if (Features.Contains(feature)) throw new InvalidOperationException("Cannot register twice.");
            Features.Add(feature);
            feature.Key.Flavour = FeatureKeyFlavour;
            
            var id =  Features.Count - 1 + IdOffset; //id may not match index
            feature.DataRepresentation = GetSettingValue(id);
            return id;
        }

        protected abstract int GetSettingValue(int id);

        public T Get(int index) => Features[index];

        public void Freeze(FeatureWorld fw)
        {
            if (IsFrozen) throw new FeatureResolverLockedException("Feature already locked.");
            FeatureWorld = fw;
            Features.TrimExcess();
            OnFreeze();
            GlobalResourceManager.Instance.FreezeResourceable(this); //so that hooks on this get resolved
            IsFrozen = true;
        }

//        public abstract override bool HasFeature(Tile tile);
//        public abstract override bool IsFeatureOnTile(Tile tile, T feature);
//        public abstract override void AddFeature(Tile tile, T feature);
//        public abstract override void RemoveFeature(Tile tile, T feature);
        protected virtual void OnFreeze() { }
        public abstract DataIdentifier[] GenerateIdentifiers();
        public abstract DataIdentifier GetIdentifier(int index);


        public IEnumerator<T> GetEnumerator()
        {
            return Features.GetEnumerator();
        }

        protected internal abstract bool ValidateData(Tile tile, T feature, bool set);

        #region Hidden

        public Key Key { get; }
        public FeatureWorld FeatureWorld { get; set; }
        protected KeyFlavour KeyFlavour { get; }
        protected KeyFlavour FeatureKeyFlavour { get; }
        public bool IsFrozen { get; protected set; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
        IEnumerable<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumerableWrapper<T,IKeyNameable>(this);
        IEnumerable<T> IResourceable<T>.GetResourceables() => this;
        Key IKeyed.Key => Key;
        

        #endregion

    }
}