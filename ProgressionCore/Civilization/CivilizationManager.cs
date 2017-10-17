using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationManager : IEnumerable<Civilization>
    {
        private readonly List<Civilization> _civilizations;
        
        
        
        public CivilizationManager(Key key, short max)
        {
            Key = key;
            Max = (short) Math.Pow(2, Math.Ceiling(Math.Log(max, 2)));
            _civilizations = new List<Civilization>(Math.Min((short) 128, Max));
            Resolver = new CivilizationFeatureResolver(this);
        }

        public IEnumerator<Civilization> GetEnumerator() => _civilizations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _civilizations.GetEnumerator();

        public void AddCivilisation(Civilization civ)
        {
            if (Count == Max) throw new InvalidOperationException("Maximum number of civilizations reached. Please contact game developer to raise limit");
            _civilizations.Add(civ);
            civ.Index = _civilizations.Count - 1;
            civ.Manager = this;
        }
        
        public Key Key { get; }
        public bool Locked { get; private set; }
        public short Count => (short) _civilizations.Count;
        public short Max { get; }
        public CivilizationFeatureResolver Resolver { get; }
        
        public void Lock()
        {
            if (Locked) throw new FeatureResolverLockedException("Civilization manager already locked");
            Locked = true;
        }
    }
}