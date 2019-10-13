using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.Civilization
{
    public class Civilization : IFeature<Civilization>, IWorldHolder
    {
        private readonly List<Civilization> _puppets = new List<Civilization>();
        private IPuppetLevel _puppetLevel;
        public Civilization Master { get; private set; }
        public bool IsPuppet => Master != null;
        public IEnumerator<Civilization> Puppets => _puppets.GetEnumerator();

        public IPuppetLevel PuppetLevel => _puppetLevel;


        public Civilization(string name, CivilizationRegister register)
        {
            Name = name;
            Register = register;
            Key = new Key(register.Key, name);
            Index = -1; //this is done to avoid to make this civ not equal another civ with index 0
            Index = Register.FreeIndex;
            Register.AddCivilisation(this);
        }

        IFeatureResolver IFeature.Resolver => Register.Resolver;

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
        public int Index { get; }
        public CivilizationRegister Register { get; }


        public Vision GetVision(Tile tile)
        {
            return tile.World.CivilizationManager.GetVision(tile, this);
        }

        public int GetLastMapUpdate(Tile tile) => tile.World.CivilizationManager.GetLastMapUpdate(tile);

        public bool IsOwning(Tile tile)
        {
            return tile.World.CivilizationManager.Register.Resolver.GetOwnerId(tile) == Index;
        }

        public void Own(Tile tile)
        {
            tile.World.CivilizationManager.SetOwner(tile, this);
        }


        #region Hidden


        protected bool Equals(Civilization other)
        {
            return Index!=-1 && Index == other.Index && Equals(Register, other.Register);
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

        public byte WorldType => Register.Resolver.WorldTypePlayerId;
    }
}