using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.City.Updates
{
    public abstract class CityUpdateBase : WorldUpdateBase
    {
        public CityUpdateBase(City city)
        {
            City = city;
        }

        public City City { get; }
    }
}