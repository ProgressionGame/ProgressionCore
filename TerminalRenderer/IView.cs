namespace Progression.TerminalRenderer
{
    public interface IView
    {
        int TopPx { get; }
        int LeftPx { get; }
        int HeightPx { get; }
        int WidthPx { get; }

        void Render();
    }
}