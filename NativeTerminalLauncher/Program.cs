using System;
using System.IO;
using System.Text;
using System.Threading;
using Progression.CCL.Generic;
using Progression.CCL.Windows;
using Progression.TerminalRenderer;
using static Progression.TerminalRenderer.Colour.Common;

namespace Progression.CCL
{
    internal static class Program
    {
        private static bool running = true;
        private static string[] ErrorDisplay;
        private static Colour ErrorColour;
        private static long ErrorUntil;
        private static string CurrentInput;
        private static int CurrentCursor;


        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += On_ProcessExit;

            new Thread(Listener).Start();


            var platform = Environment.OSVersion.Platform;
            var winNt = platform == PlatformID.Win32NT;
            IConsole con = new NativeConsole(winNt && !Console.IsOutputRedirected
                ? (IAnsiConsole) new ConhostAnsiConsole()
                : new GenericAnsiConsole());

            con.Init();
            var view = Test.CreateView(con);
            con.UseNewScreenBuffer();
            while (running) {
                view.Render();
                Thread.Sleep(32);
                if (ErrorUntil > DateTime.Now.Ticks) {
                    con.SetForegroundColour(ErrorColour);
                    int i = 0;
                    foreach (var s in ErrorDisplay) {
                        con.SetCurrentPos(con.Height - ErrorDisplay.Length + i++, 2);
                        con.Write(s);
                    }
                }
                if (!string.IsNullOrEmpty(CurrentInput)) {
                    con.SetCurrentPos(con.Height, 1);
                    con.SetForegroundColour(Lime);
                    con.Write('>');
                    con.SetForegroundColour(White);
                    con.Write(CurrentInput);
                    con.Write(' ');
                    con.SetCurrentPos(con.Height + (1+CurrentCursor)/con.Width - (1+CurrentInput.Length)/con.Width, (CurrentCursor + 1)%con.Width + 1);
                    if (DateTime.Now.Ticks % 10_000_000 < 5000000) {
                        con.Write('_');
                    }
                }
                con.Flush();
            }

            con.UseMainScreenBuffer();
            con.Write("Program closing...\n");
            con.Flush();
            Thread.Sleep(200);
        }


        public static void Listener()
        {
            Thread.CurrentThread.IsBackground = true;
            Console.TreatControlCAsInput = true;
            while (running) {
                var line = ReadLine();
                if (String.IsNullOrWhiteSpace(line)) continue;
                DisplayError("Unknown input: \n" + line);
                if (line.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
                    running = false;
                }
            }
        }

        public static string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            int num;
            int cursor = 0;
            while (true) {
                num = Console.ReadKey(true).KeyChar;
                switch (num) {
                    case 3:
                        running = false;
                        break;
                    case 1:
                        if (cursor > 0) cursor--;
                        break;
                    case 4:
                        if (cursor < sb.Length) cursor++;
                        break;
                    case 8:
                    case 19:
                    case 27:
                        if (cursor > 0) {
                            sb.Remove(cursor - 1, 1);
                            cursor--;
                        }
                        break;
                    case 23:
                        if (cursor < sb.Length) {
                            sb.Remove(cursor, 1);
                        }
                        break;
                    case 13:
                    case 10:
                        CurrentInput = null;
                        return sb.ToString();
                    default:
                        if (num>27) {
                            sb.Insert(cursor, (char) num);
                            cursor++;
                        }
                        break;
                }
                CurrentInput = sb.ToString();
                CurrentCursor = cursor;
            }
        }

        static void On_ProcessExit(object sender, EventArgs e)
        {
            running = false;
        }

        static void DisplayError(string error)
        {
            ErrorDisplay = error.Split('\n');
            ErrorUntil = DateTime.Now.Ticks + 10_000 * (1000 + error.Length * 80);
            ErrorColour = Red;
        }

        private const int VK_ESCAPE = 0x1B;
        private const int WM_KEYDOWN = 0x0100;
    }
}