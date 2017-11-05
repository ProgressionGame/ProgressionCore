namespace Progression.TerminalRenderer
{
    public abstract class View : IView
    {
        private readonly int _heightPx;
        private readonly int _widthPx;

        public View(IConsole console, int heightPx=-1, int widthPx=-1, int topPx=0, int leftPx=0)
        {
            Console = console;
            TopPx = topPx;
            LeftPx = leftPx;
            _heightPx = heightPx;
            _widthPx = widthPx;
        }
        public int TopPx { get; }
        public int LeftPx { get; }

        public int HeightPx => _heightPx==-1?Console.Height-TopPx:_heightPx;
        public int WidthPx => _widthPx==-1?Console.Width-TopPx:_widthPx;
        public abstract void Render();

        public IConsole Console { get; }
    }
}