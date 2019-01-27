using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Progression.Engine.Core.World
{
    public class HexagonGrid : IGridDirection
    {
        public readonly int X;
        public readonly int Y;
        public HexagonGrid(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Tile Transform(Tile tile)
        {
            int t1 = tile.Coordinate.X;
            int t2 = tile.Coordinate.Y;
            int s1 = t1 + X;
            int s2 = t2 + X;
            var w = tile.World;
            var height = w.Height;
            var width = w.Width;
            if (!w.WrapHorizontal && (s1 > width || s1 < 0)) return w.OutOfBounds;
            if (!w.WrapVertical && (s2 > height || s2 < 0)) return w.OutOfBounds;


            var t1Off = width - t2 / 2;
            
            s1 = (s1 + t1Off)%width;
            var r1 = 
                //TODO: delete, no conversation is probably best. does only really matter for renderer and out of bounds everyway

        } 

        public float Distance() => 1;
    }
}