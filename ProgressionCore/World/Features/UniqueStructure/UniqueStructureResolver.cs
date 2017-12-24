using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource;
using Progression.Util;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.World.Features.UniqueStructure
{
    public class UniqueStructureResolver : IFeatureResolver<IUniqueStructure> {
        private readonly List<IUniqueStructureManager<IUniqueStructure>> _managers = new List<IUniqueStructureManager<IUniqueStructure>>();
        
        

        //index (normal length 15 bitS) 
        protected internal DataIdentifier DiIndex { get; protected set; }
        //Type identifer
        protected internal DataIdentifier DiType { get; protected set; }
        
        public IEnumerator<IUniqueStructure> GetEnumerator()
        {
            return new JoinedEnumerator<IUniqueStructure>(_managers);
        }

        public void Freeze(FeatureWorld fw)
        {
            
            FeatureWorld = fw;
            foreach (var manager in _managers) {
                manager.Freeze();
            }
            IsFrozen = true;
        }

        public int Count => _managers.Count;
        public readonly byte IndexLength;
        
        public UniqueStructureResolver(byte indexLength = 15)
        {
            if (indexLength < 1 || indexLength > 15) throw new ArgumentException("Needs to be between 1 and 15", nameof(indexLength));
            IndexLength = indexLength;
        }
        

        public IUniqueStructure Get(int index)
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
        
        
        #region Hidden

        public Key Key { get; }
        public FeatureWorld FeatureWorld { get; set; }
        public bool IsFrozen { get; protected set; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
        IEnumerable<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumerableWrapper<IUniqueStructure,IKeyNameable>(this);
        IEnumerable<IUniqueStructure> IResourceable<IUniqueStructure>.GetResourceables() => this;
        Key IKeyed.Key => Key;
        

        #endregion
    }
}