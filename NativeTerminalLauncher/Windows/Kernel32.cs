using System;
using System.Runtime.InteropServices;

namespace Progression.CCL.Windows
{
    public static class Kernel32
    {
        public const int StdOutputHandle = -11;
        public const int StdInputHandle = -10;
        
        public const uint ConsoleOutModeEnableProcessedOutput  = 0x0001;
        public const uint ConsoleOutModeEnableWrapAtEolOutput  = 0x0002;
        public const uint ConsoleOutModeEnableVirtualTerminalProcessing = 0x0004;
        public const uint ConsoleOutModeDisableNewlineAutoReturn  = 0x0008;
        
        
        public const uint ConsoleInModeEnableProcessedInput   = 0x0001;
        public const uint ConsoleInModeEnableLineInput   = 0x0002;
        public const uint ConsoleInModeEnableEchoInput   = 0x0004;
        public const uint ConsoleInModeEnableMouseInput  = 0x0010;
        //public const uint ConsoleInMode  = 0x0000;
        public const uint ConsoleInModeEnableVirtualTerminalInput   = 0x0200;
        public const string IOErrorMessageGeneric = "Kernel call failed";

        [DllImport("kernel32.dll")]
        public static extern bool WriteConsole(
            IntPtr hConsoleOutput,
            string lpBuffer, uint nLength,
            out uint lpNumberOfCharsWritten,
            IntPtr lpReserved);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(
            IntPtr hConsoleHandle,
            uint dwMode);

        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleMode(
            IntPtr hConsoleHandle,
            out uint dwMode);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);
    }
}