namespace Progression.Util
{
    public struct Rect
    {
        public Rect(ushort left, ushort top, ushort width, ushort height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public ushort Left { get; }
        public ushort Top { get; }
        public ushort Width { get; }
        public ushort Height { get; }

        public bool IsInside(ushort x, ushort y)
        {
            return x >= Top && x < Top + Height && y >= Left && y < Left + Width;
        }
    }
}