using System;
using System.Collections;
using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Threading;
using Progression.Resource;
using Progression.Util;
using Progression.Util.Generics;
using Progression.Util.Keys;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationFeatureResolver : IFeatureResolver<Civilization>
    {
        protected  const int DiExtraCount = 3;
        public readonly WorldType WorldTypeBase;
        public readonly WorldType WorldTypeCiv;
        public readonly WorldType WorldTypeBoth;
        public readonly byte WorldTypePlayerId;

        //custom bit length defined by Register.Max civ count
        //only player world
        protected DataIdentifier DiOwnerId { get;  set; }

        //only player world
        //0=unknown, 1=discovered, 2= visible, 3=owned -> 2bits
        protected DataIdentifier DiCivVision { get; set; }

        //15 bits = turn
        protected DataIdentifier DiCivLastMapUpdate { get;  set; }


        //only core world
        //0=unknown, 1=discovered, 2= visible, 3=owned -> 2bits
        protected DataIdentifier[] DiBaseVision { get;  set; }

        //all arrays merged
        protected DataIdentifier[] Dis { get;  set; }

        protected internal void SetDiBaseVisionFeature(Civilization civ)
        {
            DiBaseVision[civ.Index].Feature = civ;
        }
        
        
        public CivilizationFeatureResolver(CivilizationRegister register, WorldType worldTypeBase,
            WorldType worldTypeCiv, byte worldTypePlayerId = 255)
        {
            Register = register;

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
            
        }

        protected internal virtual void PopulateDataIdentifiers()
        {
            DiOwnerId = new DataIdentifier(this, -1, (int) Math.Ceiling(Math.Log(Register.Max)), WorldTypeBoth);
            DiCivVision = new DataIdentifier(this, -2, 2, WorldTypeCiv);
            DiCivLastMapUpdate = new DataIdentifier(this, -3, 15, WorldTypeCiv);
            DiBaseVision = new DataIdentifier[Register.Max];
            Dis = new DataIdentifier[Register.Max + 3];
            for (var i = 0; i < Register.Max; i++) {
                DiBaseVision[i] = new DataIdentifier(this, i, 2, WorldTypeBase);
                Dis[i] = DiBaseVision[i];
            }
            Dis[Register.Max + DiExtraCount + DiOwnerId.Index] = DiOwnerId;
            Dis[Register.Max + DiExtraCount + DiCivVision.Index] = DiCivVision;
            Dis[Register.Max + DiExtraCount + DiCivLastMapUpdate.Index] = DiCivLastMapUpdate;
        }



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

        public void SetVision(Tile tile, Civilization civ, Vision vision, bool remoteUpdate=false)
        {
            if (vision == Vision.NotOwned) {
                throw new ArgumentException("Vision NotOwned is a placeholder vision. It cannot be set");
            }
            DataIdentifier di;
            if (WorldTypeBase[tile.World.WorldType]) {
                di = DiBaseVision[civ.Index];
            } else if (WorldTypeCiv[tile.World.WorldType]) {
                // ReSharper disable once PossibleUnintendedReferenceComparison - reference check wanted
                if (tile.World.Holder == civ) {
                    di = DiCivVision;
                } else {
                    throw new ArgumentException("Cannot set vision for foreign civilisation if local world rep");
                }
            } else {
                throw new ArgumentException("World does not have civilization vision features");
            }
            tile[di] = (int) vision;
            if (!remoteUpdate) {
                tile.World.ScheduleUpdate(new CivilisationVisionUpdate(tile.Coordinate, civ, vision));
            }
        }

        public int GetOwnerId(Tile tile)
        {
            return tile[DiOwnerId]-1;
        }

        public Civilization GetOwner(Tile tile)
        {
            var id = tile[DiOwnerId];
            return id==0||id>Register.Count?null:Register[id-1];
        }

        public int GetLastMapUpdate(Tile tile)
        {
#if DEBUG 
            if (!DiCivLastMapUpdate.WorldTypes[tile.World.WorldType])
                throw new ArgumentException(
                    "Debug build only: Last map update data is only stored in per player world.");
#endif
            return tile[DiCivLastMapUpdate]; 
        }

        public void SetOwner(Tile tile, Civilization civ, bool remoteUpdate = false)
        {
            var lastOwnerIndex = GetOwnerId(tile);
            var newOwnerIndex = civ?.Index?? -1; 
            if (lastOwnerIndex == newOwnerIndex) return;
            var newOwnerValue = newOwnerIndex + 1;//TODO i dont like these offsets - meaning is that 0 means no owner
            if (WorldTypeBase[tile.World.WorldType]) {
                if (lastOwnerIndex >= 0) {
                    //make sure that last owner is no longer marked as owning this tile
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
            
            if (!remoteUpdate) {
                tile.World.ScheduleUpdate(new CivilisationOwnershipUpdate(tile.Coordinate, civ));
            }
        }

        
        
        
        public CivilizationFeatureResolver(CivilizationRegister register)
        {
            Register = register;
        }

        public IEnumerator<Civilization> GetEnumerator() => Register.GetEnumerator();
        public void Freeze(FeatureWorld fw)
        {
            Register.Freeze();
            FeatureWorld = fw;
        }

        public int Count => Register.Count;
        public CivilizationRegister Register { get; }





        public DataIdentifier[] GenerateIdentifiers()
        {
            return Dis;
        }

        public DataIdentifier GetIdentifier(int index) => index < 0 && index + DiExtraCount >= 0
            ? Dis[index + Register.Max + DiExtraCount]
            : Dis[index];

        public Civilization Get(int index) => Register[index];

        
        #region Hidden

        public FeatureWorld FeatureWorld { get; set; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IFeature IFeatureResolver.Get(int index) => Get(index);
        IEnumerable<IKeyNameable> IResourceable.GetResourceables() => new BaseTypeEnumerableWrapper<Civilization,IKeyNameable>(this);
        IEnumerable<Civilization> IResourceable<Civilization>.GetResourceables() => this;
        Key IKeyed.Key => Register.Key;
        #endregion

        public bool IsFrozen => Register.IsFrozen;
    }

}