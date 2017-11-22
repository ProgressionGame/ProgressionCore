namespace Progression.TerminalRenderer
{
    public interface IConsole
    {
        void SetCurrentPos(int x, int y);

        void CursorUp(int n = 1);

        void CursorDown(int n = 1);

        void CursorForward(int n = 1);

        void CursorBack(int n = 1);

        void Normal();

        void Bold();

        void Underline();

        void UnderlineOff();

        void BlinkOff();

        void BlinkSlow();

        void UseNewScreenBuffer();

        void UseMainScreenBuffer();

        void ClearScreen(ClearScreenMode mode);
        void HideCursor();
        void ShowCursor();

        void SetTitle(string title);

        void SetForegroundColour(byte red, byte green, byte blue);
        void SetForegroundColour(Colour colour);

        void SetBackgroundColour(byte red, byte green, byte blue);
        void SetBackgroundColour(Colour color);

        void Write(string str);
        void Write(char c);
        void Write<T>(T o);

        void Init();

        int Flush();
        
        int Height { get; }
        int Width { get; }
    }
}