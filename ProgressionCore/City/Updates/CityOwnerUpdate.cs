using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.City.Updates
{
    public class CityOwnerUpdate : CityUpdateBase
    {
        public CityOwnerUpdate(City city, Civilization.Civilization newOwner) : base(city)
        {
            NewOwner = newOwner;
        }
        
        public Civilization.Civilization NewOwner { get; }
        
        public override void Execute(WorldInterface @on)
        {
            throw new System.NotImplementedException();
        }
    }
}