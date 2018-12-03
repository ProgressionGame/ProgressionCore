using System;
using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.Simple
{
    public class SingleFeatureResolver<T> : StaticFeatureResolver<T>, ISingleFeatureResolver<T>
        where T : class, ISimpleFeature<T>
    {
        public SingleFeatureResolver(WorldType worldType, Key key, bool optional) : base(worldType, key, optional ? 1 : 0)
        {
            
        }

        public DataIdentifier DataIdentifier { get; private set; }

        public bool HasFeature(Tile tile)
        {
            return tile[DataIdentifier] >= IdOffset;
        }

        protected override int GetSettingValue(int id)
        {
            return id;
        }

        public override DataIdentifier[] GenerateIdentifiers()
        {
            if (DataIdentifier != null || IsFrozen) {
                throw new InvalidOperationException("GenerateIdentifiers should only be called by FeatureWorld");
            }
            DataIdentifier = new DataIdentifier(this, 0, (int)Math.Ceiling(Math.Log(Count+IdOffset, 2)), WorldType);
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

        protected internal override bool ValidateData(Tile tile, T feature, bool set)
        {
            return true;
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