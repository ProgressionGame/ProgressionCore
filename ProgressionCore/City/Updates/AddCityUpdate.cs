using System;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Threading;
using Progression.Util;

namespace Progression.Engine.Core.City.Updates
{
    public class AddCityUpdate : TileUpdateBase
    {
        public AddCityUpdate(Coordinate coordinate, string name, Civilization.Civilization owner, bool justFounded) : base(coordinate)
        {
            Owner = owner;
            JustFounded = justFounded;
            Name = name;
        }
        
        public string Name { get; }
        public Civilization.Civilization Owner { get; }
        public bool JustFounded { get; }
        public override void Execute(WorldInterface @on)
        {
            throw new NotImplementedException();
        }
    }
}