using Progression.Engine.Core.World;

namespace Progression.Engine.Core.Civilization
{
    public class CivilizationManager
    {
        public CivilizationManager(CivilizationRegister register)
        {
            Register = register;
        }
        public CivilizationRegister Register { get; }
        
        
        public Vision GetVision(Tile tile, Civilization civ) => Register.Resolver.GetVision(tile, civ);

        public void SetVision(Tile tile, Civilization civ, Vision vision) => Register.Resolver.SetVision(tile, civ, vision);

        public Civilization GetOwner(Tile tile) => Register.Resolver.GetOwner(tile);

        public int GetLastMapUpdate(Tile tile) => Register.Resolver.GetLastMapUpdate(tile);

        public void SetOwner(Tile tile, Civilization civ) => Register.Resolver.SetOwner(tile, civ);
    }
}