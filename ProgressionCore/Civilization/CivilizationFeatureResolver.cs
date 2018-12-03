using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource;
using Progression.Util;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationFeatureResolver : IFeatureResolver<Civilization>
    {

        public CivilizationFeatureResolver(CivilizationManager manager)
        {
            Manager = manager;
        }

        public IEnumerator<Civilization> GetEnumerator() => Manager.GetEnumerator();
        public void Freeze(FeatureWorld fw)
        {
            Manager.Freeze();
            FeatureWorld = fw;
        }

        public int Count => Manager.Count;
        public CivilizationManager Manager { get; }





        public DataIdentifier[] GenerateIdentifiers()
        {
            return Manager.Dis;
        }

        public DataIdentifier GetIdentifier(int index) => index < 0 && index + CivilizationManager.DiExtraCount >= 0
            ? Manager.Dis[index + Manager.Max + CivilizationManager.DiExtraCount]
            : Manager.Dis[index];

        public Civilization Get(int index) => Manager[index];

        
        #region Hidden

        public FeatureWorld FeatureWorld { get; set; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
        IEnumerable<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumerableWrapper<Civilization,IKeyNameable>(this);
        IEnumerable<Civilization> IResourceable<Civilization>.GetResourceables() => this;
        Key IKeyed.Key => Manager.Key;
        #endregion

        public bool IsFrozen => Manager.IsFrozen;
    }

}