using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource.Util;
using Progression.Util;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.UniqueStructure
{
    public class UniqueStructureResolver : FeatureResolverBase<IUniqueStructure> {
        private readonly List<IUniqueStructureManager<IUniqueStructure>> _managers = new List<IUniqueStructureManager<IUniqueStructure>>();
        
        

        //index (normal length 15 bitS) 
        protected internal DataIdentifier DiIndex { get; protected set; }
        //Type identifer
        protected internal DataIdentifier DiType { get; protected set; }
        
        public override IEnumerator<IUniqueStructure> GetEnumerator()
        {
            return new JoinedEnumerator<IUniqueStructure>(_managers);
        }

        public override bool HasFeature(Tile tile)
        {
            throw new System.NotImplementedException();
        }

        public override void Freeze(FeatureWorld fw)
        {
            
            FeatureWorld = fw;
            foreach (var manager in _managers) {
                manager.Freeze();
            }
            IsFrozen = true;
        }

        public override int Count => _managers.Count;
        public readonly byte IndexLength;
        
        public UniqueStructureResolver(byte indexLength = 15) : base(null) //todo remove null
        {
            if (indexLength < 1 || indexLength > 15) throw new ArgumentException("Needs to be between 1 and 15", nameof(indexLength));
            IndexLength = indexLength;
        }

        public override bool IsFeatureOnTile(Tile tile, IUniqueStructure feature)
        {
            throw new System.NotImplementedException();
        }

        public override void AddFeature(Tile tile, IUniqueStructure feature)
        {
            throw new System.NotImplementedException();
        }


        public override void RemoveFeature(Tile tile, IUniqueStructure feature)
        {
            throw new System.NotImplementedException();
        }
        

        public override IUniqueStructure Get(int index)
        {
            throw new System.NotImplementedException();
        }

        public override DataIdentifier[] GenerateIdentifiers()
        {
            throw new System.NotImplementedException();
        }

        public override DataIdentifier GetIdentifier(int index)
        {
            throw new System.NotImplementedException();
        }
        
       
        
//        public IUniqueStructure GetResourceable(string name)
//        {
//            throw new NotImplementedException();
//        }
//
//        IKNamed IResourceable.GetResourceable(string name)
//        {
//            return GetResourceable(name);
//        }

        
        
//        IUniqueStructure IResourceable<IUniqueStructure>.GetResourceable(string name) => (IUniqueStructure) FeatureTypeKey.GetChild(name).Holder;
//        IKNamed IResourceable.GetResourceable(string name) =>(IUniqueStructure) FeatureTypeKey.GetChild(name).Holder;
//        Key IKeyed.Key => FeatureTypeKey;
    }
}