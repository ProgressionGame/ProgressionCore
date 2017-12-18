using System;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Base
{
    public class StaticSingleFeatureResolver<T> : StaticFeatureResolver<T>, ISingleFeatureResolver<T>
        where T : class, IStaticFeature<T>
    {
        public StaticSingleFeatureResolver(WorldType worldType, Key featureTypeKey, bool optional) : base(worldType, featureTypeKey, optional ? 1 : 0)
        {
            
        }

        public DataIdentifier DataIdentifier { get; private set; }

        public override bool HasFeature(Tile tile)
        {
            return tile[DataIdentifier] >= IdOffset;
        }


        public override bool IsFeatureOnTile(Tile tile, T feature)
        {
            return tile[DataIdentifier]  == feature.Id;
        }

        public override void AddFeature(Tile tile, T feature)
        {
            tile[DataIdentifier]  = (ushort) feature.Id;
            //TODO make this better
            tile.InvokeTileUpdate(this, feature, feature.Id, DataIdentifier);
        }

        public override void RemoveFeature(Tile tile, T feature)
        {
            if (IsFeatureOnTile(tile, feature)) {
                tile[DataIdentifier]  = 0;
                
                //TODO make this better
                tile.InvokeTileUpdate(this, feature, 0, DataIdentifier);
            }
        } 

        public override DataIdentifier[] GenerateIdentifiers()
        {
            if (DataIdentifier != null || IsFrozen) {
                throw new InvalidOperationException("GenerateIdentifiers should only be called by FeatureWorld");
            }
            DataIdentifier = new DataIdentifier(this, 0, (int)Math.Ceiling(Math.Log((int)Math.Ceiling(Math.Log(Count+IdOffset, 2)), 2)), WorldType);
            foreach (var feature in Features) {
                feature.DataIdentifier = DataIdentifier;
            }
            return new[] {DataIdentifier};
        }

        public override DataIdentifier GetIdentifier(int index)
        {
            switch (index) {
                    case 0:
                        return DataIdentifier;
                    default:
                        throw new IndexOutOfRangeException("For this resolver index can only be 0");
            }
        }

        public T GetFeature(Tile tile)
        {
            var index = tile[DataIdentifier]  - IdOffset;
            return index < 0 ? null : Features[index];
        }

        #region Hidden
        IFeature ISingleFeatureResolver.GetFeature(Tile tile)
        {
            return GetFeature(tile);
        }
        #endregion
        
    }
}