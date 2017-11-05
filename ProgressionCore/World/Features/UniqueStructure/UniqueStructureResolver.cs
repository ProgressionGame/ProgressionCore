using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.Util;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.World.Features.UniqueStructure
{
    public class UniqueStructureResolver : IFeatureResolver<IUniqueStructure> {
        private readonly List<IUniqueStructureManager> _managers = new List<IUniqueStructureManager>();
        
        

        //index (normal length 15 bitS) 
        protected internal DataIdentifier DiIndex { get; protected set; }
        //Type identifer
        protected internal DataIdentifier DiType { get; protected set; }
        
        public IEnumerator<IUniqueStructure> GetEnumerator()
        {
            return new JoinedEnumerator<IUniqueStructure>(_managers);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasFeature(Tile tile)
        {
            throw new System.NotImplementedException();
        }

        public void LockRegistration(FeatureWorld fw)
        {
            
            FeatureWorld = fw;
            foreach (var manager in _managers) {
                manager.Lock();
            }
        }

        Key IFeatureResolver.FeatureTypeKey => null;
        public int Count => _managers.Count;
        public FeatureWorld FeatureWorld { get; private set; }
        public readonly byte IndexLength;
        
        public UniqueStructureResolver(byte indexLength = 15)
        {
            if (indexLength < 1 || indexLength > 15) throw new ArgumentException("Needs to be between 1 and 15", nameof(indexLength));
            IndexLength = indexLength;
        }

        public bool IsFeatureOnTile(Tile tile, IUniqueStructure feature)
        {
            throw new System.NotImplementedException();
        }

        public void AddFeature(Tile tile, IUniqueStructure feature)
        {
            throw new System.NotImplementedException();
        }

        void IFeatureResolver<IUniqueStructure>.RemoveFeature(Tile tile, IUniqueStructure feature)
        {
            RemoveFeature(tile, feature);
        }

        
        private void RemoveFeature(Tile tile, IUniqueStructure feature)
        {
            throw new System.NotImplementedException();
        }
        
        IFeature IFeatureResolver.Get(int index)
        {
            return Get(index);
        }

        public IUniqueStructure Get(int index)
        {
            throw new System.NotImplementedException();
        }

        public bool IsFeatureOnTile(Tile tile, IFeature feature)
        {
            throw new System.NotImplementedException();
        }

        public void AddFeature(Tile tile, IFeature feature)
        {
            throw new System.NotImplementedException();
        }

        void IFeatureResolver.RemoveFeature(Tile tile, IFeature feature)
        {
            RemoveFeature(tile, (IUniqueStructure) feature);
        }

        public DataIdentifier[] GenerateIdentifiers()
        {
            throw new System.NotImplementedException();
        }

        public DataIdentifier GetIdentifier(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}