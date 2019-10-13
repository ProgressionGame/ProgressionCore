namespace Progression.Util
{
    public struct Rect
    {
        public Rect(Coordinate @base, ushort width, ushort height)
        {
            Base = @base;
            Width = width;
            Height = height;
        }

        public Coordinate Base { get; }
        public ushort Width { get; }
        public ushort Height { get; }

        public bool IsInside(ushort x, ushort y)
        {
            return x >= Base.X && x < Base.X + Height && y >= Base.Y && y < Base.Y + Width;
        }
    }
}