using System;
using System.Collections.Generic;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public abstract class StaticFeatureResolver<T> : FeatureResolverBase<T> where T : class, ISimpleFeature<T>
    {
        protected readonly List<T> Features = new List<T>();
        protected readonly int IdOffset;
        public readonly WorldType WorldType;

        protected StaticFeatureResolver(WorldType worldType, Key featureTypeKey, int idOffset) : base(featureTypeKey)
        {
            IdOffset = idOffset;
            WorldType = worldType;
        }

        public override int Count => Features.Count;

        public int Register(T feature)
        {
            if (IsFrozen)
                throw new FeatureResolverLockedException("Feature locked. Cannot add new features during game.");
            if (Features.Contains(feature)) throw new InvalidOperationException("Cannot register twice.");
            Features.Add(feature);
            return Features.Count - 1 + IdOffset; //id may not match index
        }

        public override T Get(int index) => Features[index];

        public override void Freeze(FeatureWorld fw)
        {
            if (IsFrozen) throw new FeatureResolverLockedException("Feature already locked.");
            FeatureWorld = fw;
            Features.TrimExcess();
            OnFreeze();
            IsFrozen = true;
            ResMan.GetInstance().FreezeResourceable(this); //so that hooks on this get resolved
        }

//        public abstract override bool HasFeature(Tile tile);
//        public abstract override bool IsFeatureOnTile(Tile tile, T feature);
//        public abstract override void AddFeature(Tile tile, T feature);
//        public abstract override void RemoveFeature(Tile tile, T feature);
        protected virtual void OnFreeze() { }
        public abstract override DataIdentifier[] GenerateIdentifiers();
        public abstract override DataIdentifier GetIdentifier(int index);


        public override IEnumerator<T> GetEnumerator()
        {
            return Features.GetEnumerator();
        }

    }
}