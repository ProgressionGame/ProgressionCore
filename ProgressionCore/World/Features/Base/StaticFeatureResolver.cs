using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public abstract class StaticFeatureResolver<T> : IFeatureResolver<T> where T : class, IStaticFeature<T>
    {
        protected readonly List<T> Features = new List<T>();
        public bool Locked { get; private set; }
        protected readonly int IdOffset;
        public readonly WorldType WorldType;

        protected StaticFeatureResolver(WorldType worldType, Key featureTypeKey, int idOffset)
        {
            FeatureTypeKey = featureTypeKey;
            IdOffset = idOffset;
            WorldType = worldType;
        }

        public Key FeatureTypeKey { get; }
        public int Count => Features.Count;
        public FeatureWorld FeatureWorld { get; private set; }

        public int Register(T feature)
        {
            if (Locked)
                throw new FeatureResolverLockedException("Feature locked. Cannot add new features during game.");
            if (Features.Contains(feature)) throw new InvalidOperationException("Cannot register twice.");
            Features.Add(feature);
            return Features.Count - 1 + IdOffset; //id may not match index
        }

        public T Get(int index) => Features[index];

        public void LockRegistration(FeatureWorld fw)
        {
            if (Locked) throw new FeatureResolverLockedException("Feature already locked.");
            FeatureWorld = fw;
            Features.TrimExcess();
            OnLock();
            Locked = true;
        }

        public abstract bool HasFeature(Tile tile);
        public abstract bool IsFeatureOnTile(Tile tile, T feature);
        public abstract void AddFeature(Tile tile, T feature);
        public abstract void RemoveFeature(Tile tile, T feature);
        public virtual void OnLock() { }
        public abstract DataIdentifier[] GenerateIdentifiers();
        public abstract DataIdentifier GetIdentifier(int index);


        public IEnumerator<T> GetEnumerator()
        {
            return Features.GetEnumerator();
        }

        #region Hidden

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
        bool IFeatureResolver.IsFeatureOnTile(Tile tile, IFeature feature) => IsFeatureOnTile(tile, (T) feature);
        void IFeatureResolver.AddFeature(Tile tile, IFeature feature) => AddFeature(tile, (T) feature);
        void IFeatureResolver.RemoveFeature(Tile tile, IFeature feature) => RemoveFeature(tile, (T) feature);

        #endregion
    }
}