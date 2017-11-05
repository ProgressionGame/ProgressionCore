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
            if ((mode & Kernel32.ConsoleModeEnableVirtualTerminalProcessing) == 0) {
                ConsoleMode = mode | Kernel32.ConsoleModeEnableVirtualTerminalProcessing;
            }
        }

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