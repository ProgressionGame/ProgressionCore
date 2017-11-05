using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Progression.CCL
{
    public class ConsoleManager
    {
        public const string AnsiEscape = "\x1b[";
        public const string AnsiForegroundColour24Bit = "38;2;";
        
        private readonly StringBuilder _sb = new StringBuilder();
        public ConsoleManager(IAnsiConsole console)
        {
            Console = console;
            
        }
        private IAnsiConsole Console { get; }
        
        
        public void SetCurrentPosX(int i) { }
        public void SetCurrentPosY(int i) { }
        public void SetCurrentPos(int x, int y) { }

        public void SetForegroundColour(byte red, byte green, byte blue)
        {
            _sb.Append(AnsiEscape);
            _sb.Append(AnsiForegroundColour24Bit);
            _sb.Append(red);
            _sb.Append(';');
            _sb.Append(green);
            _sb.Append(';');
            _sb.Append(blue);
            _sb.Append(';');
            _sb.Append('m');
        }

        public void Write(String str)
        {
            _sb.Append(str);
        }

        public void Init()
        {
            Console.EnableAnsi();
        }

        public void Flush()
        {
            System.Console.WriteLine(Console.Write(_sb.ToString()));
            _sb.Clear();
        }

    }
}