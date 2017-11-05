namespace Progression.TerminalRenderer
{
    public struct Colour
    {
        public static class Common
        {
            public static readonly Colour Black = new Colour();
            public static readonly Colour White = new Colour(255,255,255);
            public static readonly Colour Red = new Colour(red: 255);
            public static readonly Colour Green = new Colour(green: 255);
            public static readonly Colour Blue = new Colour(blue: 255);
            //I am much too lazy to expend this
        }
        
        
        
        public Colour(byte red = 0, byte green = 0, byte blue = 0)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool Equals(Colour other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Colour colour && Equals(colour);
        }
        
        public static bool operator ==(Colour c1, Colour c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Colour c1, Colour c2)
        {
            return !(c1 == c2);
        }

        public override int GetHashCode()
        {
            unchecked {
                var hashCode = Red.GetHashCode();
                hashCode = (hashCode * 397) ^ Green.GetHashCode();
                hashCode = (hashCode * 397) ^ Blue.GetHashCode();
                return hashCode;
            }
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
    }
}