using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.World.Features.Base
{
    public class StaticMultiFeatureResolver<T> : StaticFeatureResolver<T>, IMultiFeatureResolver<T>
        where T : class, IStaticFeature<T>
    {
        public StaticMultiFeatureResolver(WorldType worldType, Key featureTypeKey) : base(worldType, featureTypeKey, 0) { }

        private DataIdentifier[] _identifiers;
        

        public override bool HasFeature(Tile tile)
        {
            foreach (var feature in Features) {
                if (IsFeatureOnTile(tile, feature)) return true;
            }
            return false;
        }

        public override bool IsFeatureOnTile(Tile tile, T feature)
        {
            return tile[feature.DataIdentifier] != 0;
        }

        public override void AddFeature(Tile tile, T feature)
        {
            tile[feature.DataIdentifier] = 1;
            //TODO make this better
            tile.InvokeTileUpdate(this, feature, 1, feature.DataIdentifier);
        }

        public override void RemoveFeature(Tile tile, T feature)
        {
            tile[feature.DataIdentifier] = 0;
            //TODO make this better
            tile.InvokeTileUpdate(this, feature, 0, feature.DataIdentifier);
        }

        public override DataIdentifier[] GenerateIdentifiers()
        {
            if (_identifiers != null || Locked) {
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

        public T[] GetFeatures(Tile tile)
        {
            var result = new List<T>(Count);
            foreach (var feature in Features) {
                if (IsFeatureOnTile(tile, feature)) result.Add(feature);
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