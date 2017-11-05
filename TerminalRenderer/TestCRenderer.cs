using Progression.Engine.Core.World;

namespace Progression.TerminalRenderer
{
    public class TestCRenderer :ICRenderer
    {
        private readonly char[,] chars = new char[3,3];
        private Colour[,] _colours = new Colour[3,3];
        
        
        
        public TestCRenderer()
        {
            chars[0, 0] = '1';
            chars[0, 1] = '2';
            chars[0, 2] = '3';
            chars[1, 0] = '4';
            chars[1, 1] = '5';
            chars[1, 2] = '6';
            chars[2, 0] = '7';
            chars[2, 1] = '8';
            chars[2, 2] = '9';
            
            
            _colours[0, 0] = new Colour(255, 255, 255);
            _colours[0, 1] = new Colour(0, 255, 255);
            _colours[0, 2] = new Colour(255, 0, 255);
            _colours[1, 0] = new Colour(255, 255, 0);
            _colours[1, 1] = new Colour(0, 0, 255);
            _colours[1, 2] = new Colour(255, 0, 0);
            _colours[2, 0] = new Colour(0, 255, 0);
            _colours[2, 1] = new Colour(128, 255, 128);
            _colours[2, 2] = new Colour(255, 128, 0);
        }

        public bool Print(Tile coord, int relX, int relY, out char c, out Colour colour)
        {
            c = chars[relX, relY];
            colour = _colours[relX, relY];
            return true;
        }

        public bool Active(int n)
        {
            return true;
        }
    }
}