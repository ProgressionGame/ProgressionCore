using Progression.Engine.Core.Civilization;

namespace Progression.Engine.Core.World.Threading
{
    public class CivilisationVisionUpdate : TileUpdateBase
    {
        public CivilisationVisionUpdate(Coordinate coordinate, Civilization.Civilization civilization, Vision vision) : base(coordinate)
        {
            Civilization = civilization;
            Vision = vision;
        }
        
        
        public Civilization.Civilization Civilization { get; }
        public Vision Vision { get; }
        
        public override void Execute(WorldInterface @on)
        {
            on.CivilisationVisionUpdate(Coordinate, Civilization, Vision);
        }
    }
}