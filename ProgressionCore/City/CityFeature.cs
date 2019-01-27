using Progression.Engine.Core.World.Features.Base;
using Progression.Util.Keys;

namespace Progression.Engine.Core.City
{
    public class CityFeature : IFeature<CityFeature> {
        public CityFeature(CityFeatureResolver resolver)
        {
            Resolver = resolver;
        }


        public Key Key => Resolver.CityKey;
        public string Name => "City";
        public CityFeatureResolver Resolver { get; }

        IFeatureResolver IFeature.Resolver => Resolver;
    }
}