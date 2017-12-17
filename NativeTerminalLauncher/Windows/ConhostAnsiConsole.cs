using System;
using System.IO;
using Microsoft.Win32;

namespace Progression.CCL.Windows
{
    public class ConhostAnsiConsole : IAnsiConsole
    {
        private bool _ansi = false;
        public const string RegCurrentVersion = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
        public string WindowsVersion { get; }
        public bool Win10 { get; }
        public bool Win10CU { get; }

        public bool Ansi => _ansi && Win10CU;

        public ConhostAnsiConsole(IntPtr consoleOutPtr, IntPtr consoleInPtr)
        {
            ConsoleOutPtr = consoleOutPtr;
            ConsoleInPtr = consoleInPtr;
            WindowsVersion = Registry.GetValue(RegCurrentVersion, "ProductName", "").ToString();
            Win10 = WindowsVersion.Trim().ToLower().StartsWith("windows 10");
            if (Win10) {
                int releaseId = int.Parse(Registry.GetValue(RegCurrentVersion, "ReleaseId", "-1").ToString());
                Win10CU = releaseId >= 1703;
            }
        }

        public ConhostAnsiConsole() : this(Kernel32.GetStdHandle(Kernel32.StdOutputHandle),
            Kernel32.GetStdHandle(Kernel32.StdInputHandle)) { }

        public IntPtr ConsoleOutPtr { get; }
        public IntPtr ConsoleInPtr { get; }

        public int Write(string sLine)
        {
            if (!Kernel32.WriteConsole(ConsoleOutPtr, sLine, (uint) sLine.Length, out var written, (IntPtr) 0)
            ) return -1;
            return (int) written;
        }

        public void EnableAnsi()
        {
            var modeOut = ConsoleOutMode;
            var modeIn = ConsoleInMode;
            var resultOut = modeOut;
            var resultIn = modeIn;
            
            
            resultOut |= Kernel32.ConsoleOutModeEnableProcessedOutput;
            resultOut &= ~Kernel32.ConsoleOutModeEnableWrapAtEolOutput;
            resultIn |= Kernel32.ConsoleInModeEnableProcessedInput;
            resultIn &= ~Kernel32.ConsoleInModeEnableLineInput;
            resultIn &= ~Kernel32.ConsoleInModeEnableEchoInput;
            if (Win10CU) {
                resultOut |= Kernel32.ConsoleOutModeEnableVirtualTerminalProcessing;
                resultIn |= Kernel32.ConsoleInModeEnableVirtualTerminalInput;
            }
            
            
            if (modeOut != resultOut) {
                ConsoleOutMode = resultOut;
                _ansi = true;
            }
            if (modeIn != resultIn) {
                ConsoleInMode = resultIn;
                _ansi = true;
            }
        }

        public void EnableMouse()
        {
            var modeIn = ConsoleInMode;
            var resultIn = modeIn;
            if (Win10CU) {
                resultIn |= Kernel32.ConsoleInModeEnableMouseInput;
            }
            if (modeIn != resultIn) {
                ConsoleInMode = resultIn;
            }
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

        public uint ConsoleOutMode {
            get {
                if (!Kernel32.GetConsoleMode(ConsoleOutPtr, out var result))
                    throw new IOException(Kernel32.IOErrorMessageGeneric);
                return result;
            }
            set {
                if (!Kernel32.SetConsoleMode(ConsoleOutPtr, value))
                    throw new IOException(Kernel32.IOErrorMessageGeneric);
            }
        }
        
        public uint ConsoleInMode {
            get {
                if (!Kernel32.GetConsoleMode(ConsoleInPtr, out var result))
                    throw new IOException(Kernel32.IOErrorMessageGeneric);
                return result;
            }
            set {
                if (!Kernel32.SetConsoleMode(ConsoleInPtr, value))
                    throw new IOException(Kernel32.IOErrorMessageGeneric);
            }
        }
    }
}