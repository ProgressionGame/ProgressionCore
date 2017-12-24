using System;
using System.Collections.Generic;
using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public class MultiFeatureResolver<T> : StaticFeatureResolver<T>, IMultiFeatureResolver<T>
        where T : class, ISimpleFeature<T>
    {
        public MultiFeatureResolver(WorldType worldType, Key featureTypeKey) : base(worldType, featureTypeKey, 0) { }

        private DataIdentifier[] _identifiers;
        

        public bool HasFeature(Tile tile)
        {
            foreach (var feature in Features) {
                if (feature.HasFeature(tile)) return true;
            }
            return false;
        }
//
//        public override bool IsFeatureOnTile(Tile tile, T feature)
//        {
//            return tile[feature.DataIdentifier] != 0;
//        }
//
//        public override void AddFeature(Tile tile, T feature)
//        {
//            tile[feature.DataIdentifier] = 1;
//            //TODO make this better
//            tile.InvokeTileUpdate(this, feature, 1, feature.DataIdentifier);
//        }
//
//        public override void RemoveFeature(Tile tile, T feature)
//        {
//            tile[feature.DataIdentifier] = 0;
//            //TODO make this better
//            tile.InvokeTileUpdate(this, feature, 0, feature.DataIdentifier);
//        }

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