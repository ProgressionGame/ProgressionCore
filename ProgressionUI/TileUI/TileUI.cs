using Progression.Engine.Core.World;
using Progression.Util;

namespace Progression.UI.TileUI
{
    public interface ITileUi
    {
        void displayWorld(TileWorld world);
        void centerOn(Coordinate coordinate);
        event SimpleAction SimpleAction;
        event TileAction TileAction; //todo make each action have own event
    } 
}