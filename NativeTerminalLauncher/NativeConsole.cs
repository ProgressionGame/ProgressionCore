using System.Text;
using Progression.TerminalRenderer;

namespace Progression.CCL
{
    public class NativeConsole : IConsole
    {
        public const char AnsiEscape = '\x1b';
        public const char AnsiCSI = '[';
        public const string AnsiForegroundColour24Bit = "38;2;";
        public const string AnsiBackgroundColour24Bit = "48;2;";
        
        private readonly StringBuilder _sb = new StringBuilder();
        public NativeConsole(IAnsiConsole console)
        {
            Console = console;
            
        }
        public IAnsiConsole Console { get; }
        
        
        //public void SetCurrentPosX(int i) { }
        //public void SetCurrentPosY(int i) { }

        public void SetCurrentPos(int x, int y)
        {
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append(x);
            _sb.Append(';');
            _sb.Append(y);
            _sb.Append('H');
        }

        public void CursorUp(int n = 1) => CursorSingleArg(n, 'A');
        public void CursorDown(int n = 1) => CursorSingleArg(n, 'B');
        public void CursorForward(int n = 1) => CursorSingleArg(n, 'C');
        public void CursorBack(int n = 1) => CursorSingleArg(n, 'D');

        public void Normal() => SGR("m");
        public void Bold() => SGR("1m");
        public void Underline() => SGR("4m");
        public void UnderlineOff() => SGR("24m");
        public void BlinkOff() => SGR("25m");
        public void BlinkSlow() => SGR("5m");

        public void UseNewScreenBuffer()
        {
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append("?1049h");
        }
        public void UseMainScreenBuffer()
        {
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append("?1049l");
        }

        public void ClearScreen(ClearScreenMode mode)
        {
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append((int) mode);
            _sb.Append('J');
        }

        public void HideCursor()
        {
            Console.SetCursorVisibilityInteropt(false);
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append("?25l");
        }

        public void ShowCursor()
        {
            Console.SetCursorVisibilityInteropt(true);
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append("?25h");
        }

        public void SetTitle(string title)
        {
            if (Console.SetTitleInteropt(title)) return;
            _sb.Append(AnsiEscape);
            _sb.Append("]0;");
            _sb.Append(title);
            _sb.Append('\b');
        }
        
        private void CursorSingleArg(int n, char c)
        {
            
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append(n);
            _sb.Append(c);
        }


        private void AppendNumberOrOmit(byte n)
        {
            if (n > 0) _sb.Append(n);
        }

        public void SetForegroundColour(byte red, byte green, byte blue)
        {
            SGR(AnsiForegroundColour24Bit);
            AppendNumberOrOmit(red);
            _sb.Append(';');
            AppendNumberOrOmit(green);
            _sb.Append(';');
            AppendNumberOrOmit(blue);
            _sb.Append(';');
            _sb.Append('m');
        }

        public void SetForegroundColour(Colour colour) => SetForegroundColour(colour.Red, colour.Green, colour.Blue);

        private void SGR(string sgr)
        {
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiCSI);
            _sb.Append(sgr);
        }
        

        public void SetBackgroundColour(byte red, byte green, byte blue)
        {
            SGR(AnsiBackgroundColour24Bit);
            AppendNumberOrOmit(red);
            _sb.Append(';');
            AppendNumberOrOmit(green);
            _sb.Append(';');
            AppendNumberOrOmit(blue);
            _sb.Append(';');
            _sb.Append('m');
        }

        public void SetBackgroundColour(Colour color) => SetBackgroundColour(color.Red, color.Green, color.Blue);

        public void Write(string str)
        {
            _sb.Append(str);
        }

        public void Write(char c)
        {
            _sb.Append(c);
        }

        public void Write<T>(T o)
        {
            Write(o.ToString());
        }

        public void Init()
        {
            Console.EnableAnsi();
        }

        public int Flush()
        {
            var res =Console.Write(_sb.ToString());
            _sb.Clear();
            return res;
        }

        public int Height => Console.Height;
        public int Width => Console.Width;
    }
}