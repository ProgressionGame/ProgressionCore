using System;
using System.Collections.Generic;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.Civilization
{
    public class Civilization : IFeature<Civilization>
    {
        private readonly List<Civilization> _puppets = new List<Civilization>();
        private IPuppetLevel _puppetLevel;
        public Civilization Master { get; private set; }
        public bool IsPuppet => Master != null;
        public IEnumerator<Civilization> Puppets => _puppets.GetEnumerator();

        public IPuppetLevel PuppetLevel => _puppetLevel;


        public Civilization(string name, CivilizationManager manager)
        {
            Name = name;
            Manager = manager;
            Key = new Key(manager.Key, name);

            Index = Manager.AddCivilisation(this);
        }

        IFeatureResolver IFeature.Resolver => Manager.Resolver;

        public virtual void AddPuppet(Civilization civ, IPuppetLevel puppetLevel)
        {
            civ.Master?._puppets.Remove(civ);
            civ.Master = this;
            civ._puppetLevel = puppetLevel.Clone();
            _puppets.Add(civ);
        }

        public virtual void MakeIndependent()
        {
            Master?._puppets.Remove(this);
            _puppetLevel = null;
            Master = null;
        }

        public string Name { get; }
        public Key Key { get; }
        public int Index { get;}
        public CivilizationManager Manager { get; }
        //only player world
        public DataIdentifier PlayerDataIdentifierVision { get; protected internal set; } //0=unknown, 1=discovered, 2= visible, 3=owned //TODO move to manager
        public DataIdentifier PlayerDataIdentifierLastMapUpdate { get; protected internal set; } //15 bits = turn
        //only core world
        public DataIdentifier CoreDataIdentifierVision { get; protected internal set; } //0=unknown, 1=discovered, 2= visible, 3=owned
        public DataIdentifier CoreDataIdentifierOwnerId { get; protected internal set; } //custom bit length defined by max civ count
        
        public Vision GetVision(Tile tile)
        {
            return Manager.GetVision(tile, this);
        }


        #region Hidden
        bool IFeature.HasFeature(Tile tile)
        {
            return ((IFeatureResolver<Civilization>) Manager.Resolver).IsFeatureOnTile(tile, this);
        }

        void IFeature.AddFeature(Tile tile)
        {
            ((IFeatureResolver<Civilization>) Manager.Resolver).AddFeature(tile, this);
        }

        void IFeature.RemoveFeature(Tile tile)
        {
            ((IFeatureResolver<Civilization>) Manager.Resolver).RemoveFeature(tile, this);
        }
        
        
        protected bool Equals(Civilization other)
        {
            return Index == other.Index && Equals(Manager, other.Manager);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Civilization) obj);
        }

        public override int GetHashCode() => Index;

        #endregion
    }
}