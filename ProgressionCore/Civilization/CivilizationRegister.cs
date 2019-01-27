using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Resource;
using Progression.Util;
using Progression.Util.Keys;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationRegister : IEnumerable<Civilization>, IFrozen, IKeyed
    {
        internal readonly List<Civilization> _civilizations;
        public CivilizationRegister(Key key, short max, WorldType worldTypeBase,
            WorldType worldTypeCiv, byte worldTypePlayerId = 255)
        {
            Key = key;
            
            key.Flavour = key.Flavour ?? new KeyFlavour(this);
            KeyFlavour = key.Flavour;
            FeatureKeyFlavour= new KeyFlavour(this);
            Max = (short) (Math.Pow(2, Math.Ceiling(Math.Log(max, 2)))-1); //this is one lower because 0 -> not owned
            _civilizations = new List<Civilization>(Math.Min((short) 128, Max));
            
            Resolver = new CivilizationFeatureResolver(this, worldTypeBase, worldTypeCiv, worldTypePlayerId);

            //set all Dis
            Resolver.PopulateDataIdentifiers();
        }
        
        
        public Key Key { get; }
        public bool IsFrozen { get; private set; }
        public short Count => (short) _civilizations.Count;
        protected internal short FreeIndex => Count;
        public short Max { get; }
        public CivilizationFeatureResolver Resolver { get; }

        public void Freeze()
        {
            if (IsFrozen) throw new FeatureResolverLockedException("Civilization register already locked");
            GlobalResourceManager.Instance.FreezeResourceable(Resolver);
            IsFrozen = true;
        }
        
        protected internal KeyFlavour KeyFlavour { get; }
        protected internal KeyFlavour FeatureKeyFlavour { get; }
        
        
        public IEnumerator<Civilization> GetEnumerator() => _civilizations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _civilizations.GetEnumerator();
        
        
        protected internal void AddCivilisation(Civilization civ)
        {
            if (Count == Max)
                throw new InvalidOperationException(
                    "Register.Maximum number of Civilizations reached. Please contact game developer to raise limit. (it may be possible to configure ingame or per configuration file. To raise limit in savefile please search for such a tool or ask for it to be created.");
            if (_civilizations.Contains(civ)) throw new InvalidOperationException("Cannot register twice.");
            if (FreeIndex != civ.Index) throw new ArgumentException("Weird civ. FreeIndex does not match Civ index");
            Resolver.SetDiBaseVisionFeature(civ);
            _civilizations.Add(civ);
            civ.Key.Flavour = FeatureKeyFlavour;
            
            
            if (IsFrozen) GlobalResourceManager.Instance.OnNewResourceable(Resolver, civ);
        }


        public Civilization this[int index] => _civilizations[index];

    }
}