using System;
using System.Collections.Generic;
using Progression.Engine.Core.World;
using Progression.Util.Keys;

namespace Progression.Engine.Core.City
{
    public class CityManager
    {
        private readonly Dictionary<Coordinate, City> _cities;
        
        public CityManager(CityFeatureResolver resolver)
        {
            Resolver = resolver;
            _cities = new Dictionary<Coordinate, City>();
        }

        public CityFeatureResolver Resolver { get; }

        public bool HasCity(Tile tile)
        {
            return Resolver.HasCity(tile);
        }
        
        public City GetCity(Tile tile)
        {
            if (_cities.TryGetValue(tile.Coordinate, out var result)) throw new ArgumentException("No city on this tile");
            return result;
        }

        public void AddCity(City city, bool founded, bool remoteUpdate = false)
        {
            if (HasCity(city.Tile)) throw new ArgumentException("There is already a city");
            _cities.Add(city.Tile.Coordinate, city);
            if (!remoteUpdate) city.Tile.World.ScheduleUpdate(new AddCityUpdate(city.Tile.Coordinate, city.Name, city.Owner, founded));
        }
    }
}