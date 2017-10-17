using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;

namespace Progression.Engine.Core.Civilization
{
    public class Civilization : IFeature<Civilization>
    {
        public Civilization()
        {
            Index = -1;
        }


        bool IFeature.HasFeature(Tile tile)
        {
            throw new System.NotImplementedException();
        }

        void IFeature.AddFeature(Tile tile)
        {
            throw new System.NotImplementedException();
        }

        void IFeature.RemoveFeature(Tile tile)
        {
            throw new System.NotImplementedException();
        }

        IFeatureResolver IFeature.Resolver => Manager.Resolver;

        public string Name { get; }
        public int Index { get; internal set; }
        public CivilizationManager Manager { get; internal set; }
    }
}