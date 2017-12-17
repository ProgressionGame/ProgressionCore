namespace Progression.TerminalRenderer.Ansi
{
    public class SizeEvent : AnsiInputEvent
    {
        public SizeEvent(int height, int width)
        {
            Width = width;
            Height = height;
        }
        
        public int Width { get; }
        public int Height { get; }
    }
}