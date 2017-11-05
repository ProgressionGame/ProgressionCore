using System;
using System.IO;

namespace Progression.CCL.Windows
{
    public class ConhostAnsiConsole : IAnsiConsole
    {
        
        
        public ConhostAnsiConsole(IntPtr consolePtr)
        {
            ConsolePtr = consolePtr;
        }

        public ConhostAnsiConsole() : this(Kernel32.GetStdHandle(Kernel32.StdOutputHandle)) { }

        public IntPtr ConsolePtr { get; }
     
        public int Write(string sLine)
        {
            if (!Kernel32.WriteConsole(ConsolePtr, sLine, (uint)sLine.Length, out var written, (IntPtr)0)) return -1;
            return (int)written;
        }

        public void EnableAnsi()
        {
            var mode = ConsoleMode;
            var result = mode;
            result |= Kernel32.ConsoleModeEnableVirtualTerminalProcessing;
            result |= Kernel32.ConsoleModeDisableNewlineAutoReturn;
            if (mode != result) {
                ConsoleMode = result;
            }
        }

        public void EnableMouse()
        {
            throw new NotImplementedException();
        }

        public bool SetTitleInteropt(string title)
        {
            Console.Title = title;
            return true;
        }

        public bool SetCursorVisibilityInteropt(bool value)
        {
            Console.CursorVisible = value;
            return true;
        }

        //this should not be done this way. (can be attached to different console handle)
        public int Height => Console.WindowHeight;
        public int Width => Console.WindowWidth;

        public uint ConsoleMode {
            get {
                if (!Kernel32.GetConsoleMode(ConsolePtr, out var result)) throw new IOException(Kernel32.IOErrorMessageGeneric);
                return result;
            }
            set {
                if(!Kernel32.SetConsoleMode(ConsolePtr, value)) throw new IOException(Kernel32.IOErrorMessageGeneric);
            } 
        }
    }
}