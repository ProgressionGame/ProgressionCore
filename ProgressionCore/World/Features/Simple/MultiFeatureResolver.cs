using System;
using System.Collections.Generic;
using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public class MultiFeatureResolver<T> : StaticFeatureResolver<T>, IMultiFeatureResolver<T>
        where T : class, ISimpleFeature<T>
    {
        public MultiFeatureResolver(WorldType worldType, Key key) : base(worldType, key, 0) { }

        private DataIdentifier[] _identifiers;
        

        public bool HasFeature(Tile tile)
        {
            foreach (var feature in Features) {
                if (feature.HasFeature(tile)) return true;
            }
            return false;
        }
        

        protected override int GetSettingValue(int id)
        {
            return 1;
        }

        public override DataIdentifier[] GenerateIdentifiers()
        {
            if (_identifiers != null || IsFrozen) {
                throw new InvalidOperationException("GenerateIdentifiers should only be called by FeatureWorld");
            }
            _identifiers = new DataIdentifier[Count];
            for (var i = 0; i < Features.Count; i++) {
                _identifiers[i] = new DataIdentifier(this, Features[i], i, 1, WorldType);
                Features[i].DataIdentifier = _identifiers[i];
            }
            return _identifiers;
        }

        public override DataIdentifier GetIdentifier(int index)
        {
            throw new NotImplementedException();
        }

        protected internal override bool ValidateData(Tile tile, T feature, bool set)
        {
            return true;
        }

        public T[] GetFeatures(Tile tile)
        {
            var result = new List<T>(Count);
            foreach (var feature in Features) {
                if (feature.HasFeature(tile)) result.Add(feature);
            }
            return result.ToArray();
        }


        #region Hidden
        IFeature[] IMultiFeatureResolver.GetFeatures(Tile tile)
        {
            // ReSharper disable once CoVariantArrayConversion
            return GetFeatures(tile);
        }
        #endregion
    }
}