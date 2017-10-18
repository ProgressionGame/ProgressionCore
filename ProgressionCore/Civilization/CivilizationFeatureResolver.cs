using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationFeatureResolver : IFeatureResolver<Civilization>
    {
        public CivilizationFeatureResolver(CivilizationManager manager)
        {
            Manager = manager;
        }

        public IEnumerator<Civilization> GetEnumerator() => Manager.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Manager.GetEnumerator();
        public void LockRegistration(FeatureWorld fw)
        {
            Manager.Lock();
            FeatureWorld = fw;
        }

        public Key FeatureTypeKey => Manager.Key;
        public int Count => Manager.Count;
        public FeatureWorld FeatureWorld { get; private set; }
        public CivilizationManager Manager { get; }





        public DataIdentifier[] GenerateIdentifiers()
        {
            throw new NotImplementedException();
        }

        public DataIdentifier GetIdentifier(int index)
        {
            throw new NotImplementedException();
        }

        public Civilization Get(int index) => Manager[index];

        
        #region Hidden

        IFeature IFeatureResolver.Get(int index) => Get(index);
        bool IFeatureResolver.IsFeatureOnTile(Tile tile, IFeature feature) => ((IFeatureResolver<Civilization>) this).IsFeatureOnTile(tile, (Civilization)feature);
        void IFeatureResolver.AddFeature(Tile tile, IFeature feature) => ((IFeatureResolver<Civilization>) this).AddFeature(tile, (Civilization)feature);
        void IFeatureResolver.RemoveFeature(Tile tile, IFeature feature) => ((IFeatureResolver<Civilization>) this).RemoveFeature(tile, (Civilization)feature);

        bool IFeatureResolver<Civilization>.IsFeatureOnTile(Tile tile, Civilization feature)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }

        void IFeatureResolver<Civilization>.AddFeature(Tile tile, Civilization feature)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }

        void IFeatureResolver<Civilization>.RemoveFeature(Tile tile, Civilization feature)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }

        bool IFeatureResolver.HasFeature(Tile tile)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }
        #endregion
        
    }

}