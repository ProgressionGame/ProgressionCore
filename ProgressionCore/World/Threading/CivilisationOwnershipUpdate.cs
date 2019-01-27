namespace Progression.Engine.Core.World.Threading
{
    public class CivilisationOwnershipUpdate : TileUpdateBase
    {
        public CivilisationOwnershipUpdate(Coordinate coordinate, Civilization.Civilization civilization) : base(coordinate)
        {
            Civilization = civilization;
        }
        
        
        public Civilization.Civilization Civilization { get; }
        
        public override void Execute(WorldInterface @on)
        {
            throw new System.NotImplementedException();
        }
    }
}