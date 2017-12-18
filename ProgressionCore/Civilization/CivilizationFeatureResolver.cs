using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource.Util;
using Progression.Util;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationFeatureResolver : FeatureResolverSpecialisedBase<Civilization>
    {

        public CivilizationFeatureResolver(CivilizationManager manager) : base(manager.Key)
        {
            Manager = manager;
        }

        public override IEnumerator<Civilization> GetEnumerator() => Manager.GetEnumerator();
        public override void Freeze(FeatureWorld fw)
        {
            Manager.Freeze();
            FeatureWorld = fw;
        }

        public override int Count => Manager.Count;
        public FeatureWorld FeatureWorld { get; private set; }
        public CivilizationManager Manager { get; }





        public override DataIdentifier[] GenerateIdentifiers()
        {
            return Manager.Dis;
        }

        public override DataIdentifier GetIdentifier(int index) => index < 0 && index + CivilizationManager.DiExtraCount >= 0
            ? Manager.Dis[index + Manager.Max + CivilizationManager.DiExtraCount]
            : Manager.Dis[index];

        public override Civilization Get(int index) => Manager[index];

        
        #region Hidden
        protected override bool IsFeatureOnTile(Tile tile, Civilization feature)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }

        protected override void AddFeature(Tile tile, Civilization feature)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }

        protected override void RemoveFeature(Tile tile, Civilization feature)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }

        protected override bool HasFeature(Tile tile)
        {
            throw new NotImplementedException("This operation is undefined for this object");
        }
        #endregion

        public override bool IsFrozen {
            get => Manager.IsFrozen;
            protected set => throw new NotImplementedException();
        }
    }

}