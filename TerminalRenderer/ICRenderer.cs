using Progression.Engine.Core.World;

namespace Progression.TerminalRenderer
{
    public interface ICRenderer
    {
        bool Print(Tile coord, int relX, int relY, out char c, out Colour colour);
        bool Active(int n);

    }
} 