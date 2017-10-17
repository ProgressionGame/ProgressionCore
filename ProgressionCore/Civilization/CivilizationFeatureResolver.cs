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





        #region Hidden
        bool IFeatureResolver.IsFeatureOnTile(Tile tile, IFeature feature)
        {
            throw new System.NotImplementedException();
        }

        void IFeatureResolver.AddFeature(Tile tile, IFeature feature)
        {
            throw new System.NotImplementedException();
        }

        void IFeatureResolver.RemoveFeature(Tile tile, IFeature feature)
        {
            throw new System.NotImplementedException();
        }

        public DataIdentifier[] GenerateIdentifiers()
        {
            throw new System.NotImplementedException();
        }

        public DataIdentifier GetIdentifier(int index)
        {
            throw new System.NotImplementedException();
        }

        bool IFeatureResolver<Civilization>.IsFeatureOnTile(Tile tile, Civilization feature)
        {
            throw new System.NotImplementedException();
        }

        void IFeatureResolver<Civilization>.AddFeature(Tile tile, Civilization feature)
        {
            throw new System.NotImplementedException();
        }

        void IFeatureResolver<Civilization>.RemoveFeature(Tile tile, Civilization feature)
        {
            throw new System.NotImplementedException();
        }

        bool IFeatureResolver.HasFeature(Tile tile)
        {
            throw new System.NotImplementedException();
        }
        #endregion
        
    }

}