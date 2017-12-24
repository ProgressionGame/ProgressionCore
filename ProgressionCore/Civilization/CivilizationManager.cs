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
    public class CivilizationManager : IEnumerable<Civilization>, IFrozen, IKeyFlavourable
    {
        protected internal const int DiExtraCount = 3;
        private readonly List<Civilization> _civilizations;
        public readonly WorldType WorldTypeBase;
        public readonly WorldType WorldTypeCiv;
        public readonly WorldType WorldTypeBoth;
        public readonly byte WorldTypePlayerId;

        //custom bit length defined by max civ count
        protected internal DataIdentifier DiOwnerId { get; protected set; }

        //only player world
        //0=unknown, 1=discovered, 2= visible, 3=owned -> 2bits
        protected internal DataIdentifier DiCivVision { get; protected set; }

        //15 bits = turn
        protected internal DataIdentifier DiCivastMapUpdate { get; protected set; }


        //only core world
        //0=unknown, 1=discovered, 2= visible, 3=owned -> 2bits
        protected internal DataIdentifier[] DiBaseVision { get; protected set; }

        //all arrays merged
        protected internal DataIdentifier[] Dis { get; private set; }


        public CivilizationManager(Key key, short max, WorldType worldTypeBase = default(WorldType),
            WorldType worldTypeCiv = default(WorldType), byte worldTypePlayerId = 255)
        {
            Key = key;
            KeyFlavour = new KeyFlavour(this);
            key.Flavour = KeyFlavour;
            Max = (short) (Math.Pow(2, Math.Ceiling(Math.Log(max, 2)))-1); //this is one lower because 0 -> not owned
            _civilizations = new List<Civilization>(Math.Min((short) 128, Max));
            Resolver = new CivilizationFeatureResolver(this);

            //World type
            WorldTypeBase = worldTypeBase.Valid ? worldTypeBase : WorldType.Base;
            WorldTypeCiv = worldTypeCiv.Valid ? worldTypeCiv : WorldType.Civ;
            WorldTypeBoth = WorldTypeBase + WorldTypeCiv;
            if (worldTypePlayerId == 255) {
                for (byte i = 0; i < WorldTypeCiv.Size; i++) {
                    if (WorldTypeCiv[i]) {
                        WorldTypePlayerId = i;
                        break;
                    }
                }
            } else {
                WorldTypePlayerId = worldTypePlayerId;
            }
            if (!WorldTypeCiv[WorldTypePlayerId]) {
                throw new ArgumentException("Must be a valid player world", nameof(worldTypePlayerId));
            }
            
            //set all Dis
            // ReSharper disable once VirtualMemberCallInConstructor
            PopulateDataIdenfiers();
        }

        protected virtual void PopulateDataIdenfiers()
        {
            DiOwnerId = new DataIdentifier(Resolver, -1, (int) Math.Ceiling(Math.Log(Max)), WorldTypeBoth);
            DiCivVision = new DataIdentifier(Resolver, -2, 2, WorldTypeCiv);
            DiCivastMapUpdate = new DataIdentifier(Resolver, -3, 15, WorldTypeCiv);
            DiBaseVision = new DataIdentifier[Max];
            Dis = new DataIdentifier[Max + 3];
            for (var i = 0; i < Max; i++) {
                DiBaseVision[i] = new DataIdentifier(Resolver, i, 2, WorldTypeBase);
                Dis[i] = DiBaseVision[i];
            }
            Dis[Max + DiExtraCount + DiOwnerId.Index] = DiOwnerId;
            Dis[Max + DiExtraCount + DiCivVision.Index] = DiCivVision;
            Dis[Max + DiExtraCount + DiCivastMapUpdate.Index] = DiCivastMapUpdate;
        }

        public IEnumerator<Civilization> GetEnumerator() => _civilizations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _civilizations.GetEnumerator();

        protected internal void AddCivilisation(Civilization civ)
        {
            if (Count == Max)
                throw new InvalidOperationException(
                    "Maximum number of Civilizations reached. Please contact game developer to raise limit. (it may be possible to configure ingame or per configuration file. To raise limit in savefile please search for such a tool or ask for it to be created.");
            if (_civilizations.Contains(civ)) throw new InvalidOperationException("Cannot register twice.");
            if (FreeIndex != civ.Index) throw new ArgumentException("Weird civ. FreeIndex does not match Civ index");
            DiBaseVision[civ.Index].Feature = civ;
            _civilizations.Add(civ);
            if (IsFrozen) ResMan.GetInstance().OnNewResourceable(Resolver, civ);
        }

        public Key Key { get; }
        public bool IsFrozen { get; private set; }
        public short Count => (short) _civilizations.Count;
        protected internal short FreeIndex => Count;
        public short Max { get; }
        public CivilizationFeatureResolver Resolver { get; }

        public void Freeze()
        {
            if (IsFrozen) throw new FeatureResolverLockedException("Civilization manager already locked");
            ResMan.GetInstance().FreezeResourceable(Resolver);
            IsFrozen = true;
        }

        public Civilization this[int index] => _civilizations[index];

        public Vision GetVision(Tile tile, Civilization civ)
        {
            DataIdentifier di;
            if (WorldTypeBase[tile.World.WorldType]) {
                di = DiBaseVision[civ.Index];
            } else if (WorldTypeCiv[tile.World.WorldType]) {
                // ReSharper disable once PossibleUnintendedReferenceComparison - reference check wanted
                if (tile.World.Holder == civ) {
                    di = DiCivVision;
                } else {
                    return civ.Index == GetOwnerId(tile) ? Vision.Owned : Vision.NotOwned;
                }
            } else {
                throw new ArgumentException("World does not have civilization vision features");
            }
            return (Vision) tile[di];
        }

        public int GetOwnerId(Tile tile)
        {
            return tile[DiOwnerId]-1;
        }

        public Civilization GetOwner(Tile tile)
        {
            var id = tile[DiOwnerId];
            return id==0||id>_civilizations.Count?null:_civilizations[id-1];
        }

        public int GetLastMapUpdate(Tile tile)
        {
#if DEBUG 
            if (!DiCivastMapUpdate.WorldTypes[tile.World.WorldType])
                throw new ArgumentException(
                    "Debug build only: Last map update data is only stored in per player world.");
#endif
            return tile[DiCivastMapUpdate]; 
        }

        public void SetOwner(Tile tile, Civilization civ)
        {
            var lastOwnerIndex = GetOwnerId(tile);
            var newOwnerIndex = civ?.Index?? -1;
            if (lastOwnerIndex == newOwnerIndex) return;
            var newOwnerValue = newOwnerIndex + 1;
            if (WorldTypeBase[tile.World.WorldType]) {
                if (lastOwnerIndex >= 0) {
                    //this need to be visible as the civ would otherwise never be notified that they lost control over this land
                    tile[DiBaseVision[lastOwnerIndex]] = (int) Vision.Visible; 
                }
                tile[DiBaseVision[newOwnerIndex]] = (int) Vision.Owned;
            } else if (WorldTypeCiv[tile.World.WorldType]) {
                if (tile.World.Holder is Civilization worldCiv) {
                    if (worldCiv.Index == lastOwnerIndex) {
                        tile[DiCivVision] = (int) Vision.Visible;
                    }
                    if (worldCiv.Index == newOwnerIndex) {
                        tile[DiCivVision] = (int) Vision.Owned;
                    }
                } else {
                    throw new InvalidOperationException("Civ world is not owned by civ");
                }
            }
            tile[DiOwnerId] = newOwnerValue;
        }

        
        
        protected internal KeyFlavour KeyFlavour { get; }
        KeyFlavour IKeyFlavourable.KeyFlavour => KeyFlavour;
    }
}