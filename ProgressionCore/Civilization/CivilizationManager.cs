﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Progression.Engine.Core.Keys;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationManager : IEnumerable<Civilization>
    {
        private readonly List<Civilization> _civilizations;
        public readonly WorldType InternalType;
        public readonly WorldType PlayerType;
        
        
        
        
        public CivilizationManager(Key key, short max, WorldType internalType = default(WorldType), WorldType playerType = default(WorldType))
        {
            Key = key;
            InternalType = internalType;
            PlayerType = playerType;
            Max = (short) Math.Pow(2, Math.Ceiling(Math.Log(max, 2)));
            _civilizations = new List<Civilization>(Math.Min((short) 128, Max));
            Resolver = new CivilizationFeatureResolver(this);
            InternalType = internalType.Valid ? internalType : WorldType.Internal;
            InternalType = playerType.Valid ? playerType : WorldType.Player;
        }

        public IEnumerator<Civilization> GetEnumerator() => _civilizations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _civilizations.GetEnumerator();

        public int AddCivilisation(Civilization civ)
        {
            if (Count == Max) throw new InvalidOperationException("Maximum number of Civilizations reached. Please contact game developer to raise limit. (it may be possible to configure ingame or per configuration file. To raise limit in savefile please look out or ask for such a tool.");
            if (_civilizations.Contains(civ)) throw new InvalidOperationException("Cannot register twice.");
            _civilizations.Add(civ);
            return _civilizations.Count - 1;
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

        public Civilization this[int index] => _civilizations[index];

        protected DataIdentifier GetVisionDataIdentifier(Tile tile, Civilization civ)
        {
            if (InternalType[tile.World.WorldType]) {
                return civ.CoreDataIdentifierVision;
            }
            if (PlayerType[tile.World.WorldType]) {
                return civ.PlayerDataIdentifierVision;
            }
            throw new ArgumentException("World does not have civilization vision features");
        }
        
        public Vision GetVision(Tile tile, Civilization civ)
        {
            return (Vision) tile[GetVisionDataIdentifier(tile, civ)];
        }
        
    }
}