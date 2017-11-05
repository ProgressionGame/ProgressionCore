using System;
using System.Runtime.InteropServices;

namespace Progression.CCL.Windows
{
    public static class Kernel32
    {
        public const int StdOutputHandle = -11;
        public const uint ConsoleModeEnableVirtualTerminalProcessing = 0x0004;
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