using System;
using Progression.Engine.Core.City.Updates;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.City
{
    public class City
    {
        public City(Tile tile, string name, Civilization.Civilization owner)
        {
            Tile = tile;
            Name = name;
            Owner = owner;
        }
        public Tile Tile{ get; private set; }
        public string Name { get; private set; }
        public Civilization.Civilization Owner { get; private set; }
        
        public void SetOwner(Civilization.Civilization owner, bool remoteUpdate = false)
        {
            Owner = owner;
            if (!remoteUpdate)Tile.World.ScheduleUpdate(new CityOwnerUpdate(this, owner));
        }
        public void SetName(string name, bool remoteUpdate = false)
        {
            Name = name;
            if (!remoteUpdate)Tile.World.ScheduleUpdate(new CityNameUpdate(this, name));
        }

       
    }
}