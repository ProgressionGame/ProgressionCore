using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.City
{
    public class CityNameUpdate : CityUpdateBase
    {
        public CityNameUpdate(City city, string newName) : base(city)
        {
            NewName = newName;
        }
        
        public string NewName { get; }
        public override void Execute(WorldInterface @on)
        {
            throw new System.NotImplementedException();
        }
    }
}