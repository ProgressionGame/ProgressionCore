using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Progression.CCL.Generic;
using Progression.CCL.Windows;
using Progression.TerminalRenderer;
using Progression.TerminalRenderer.Ansi;
using static Progression.TerminalRenderer.Colour.Common;

namespace Progression.CCL
{
    internal static class Program
    {
        private static bool _running = true;
        private static bool _render = true;
        private static string[] _errorDisplay;
        private static Colour _errorColour;
        private static long _errorUntil;
        private static string _currentInput;
        private static int _currentCursor;
        private static NativeConsole con;


        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += On_ProcessExit;

            Console.OutputEncoding = Encoding.UTF8; //unicode not supported by mono correctly
            Console.InputEncoding = Encoding.UTF8;
            
            //new Thread(Listener).Start();

            var platform = Environment.OSVersion.Platform;
            var winNt = platform == PlatformID.Win32NT;
            con = new NativeConsole(winNt && !Console.IsOutputRedirected
                ? (IAnsiConsole) new ConhostAnsiConsole()
                : new GenericAnsiConsole());

            con.Init();
            
            Console.WriteLine(Console.OutputEncoding);

            TestRendering();
            return;

            var ansi = new AnsiInputConverter();
            while (true) {
//                if (!Console.KeyAvailable) {
//                    Thread.Sleep(new TimeSpan(20000));
//                    continue;
//                }
                char c = ansi.Read(false);
                Console.Write(c);
                var ev = ansi.DequeueEvent();
                Console.Write(ev?.ToString());

            }
            
            
            
            
            
////            bool last = false;
////            var sw = new Stopwatch();
//            while (true) {
////                if (!Console.KeyAvailable) {
////                    if (!last) {
////                        sw.Start();
////                        last = true;
////                    }
////                    Thread.Sleep(new TimeSpan(10000));
////                    Console.Write('.');
////                    continue;
////                }
////                if (last) {
////                    Console.WriteLine($"{sw.ElapsedMilliseconds}");
////                    sw.Reset();
////                    last = false;
////                }
//                var c = Console.ReadKey(true);
//                var shift = (c.Modifiers & ConsoleModifiers.Shift) != 0 ? "Shift" : "NoShift";
//                var control = (c.Modifiers & ConsoleModifiers.Control) != 0 ? "Control" : "NoControl";
//                var alt = (c.Modifiers & ConsoleModifiers.Alt) != 0 ? "Alt" : "NoAlt";
//                Console.Write($"'{c.KeyChar}' {(int)c.KeyChar}, {c.Key}, {shift}, {control}, {alt} \n");
//            }
            
            
            
        }


        public static void TestRendering()
        {
            con.SetForegroundColour(ForestGreen);
            con.Write("Starting...\n");
            con.Write(Console.In.GetType());
            con.Write('\n');
            con.Flush();
            var view = Test.CreateView(con);
            con.UseNewScreenBuffer();
            if (_render)view.Render();
            while (_running) {
                if (_errorUntil > DateTime.Now.Ticks) {
                    con.SetForegroundColour(_errorColour);
                    int i = 0;
                    foreach (var s in _errorDisplay) {
                        con.SetCurrentPos(con.Height - _errorDisplay.Length + i++, 2);
                        con.Write(s);
                    }
                }
                if (!string.IsNullOrEmpty(_currentInput)) {
                    con.SetCurrentPos(con.Height, 1);
                    con.SetForegroundColour(Lime);
                    con.Write('>');
                    con.SetForegroundColour(White);
                    con.Write(_currentInput);
                    con.Write(' ');
                    con.SetCurrentPos(con.Height + (1+_currentCursor)/con.Width - (1+_currentInput.Length)/con.Width, (_currentCursor + 1)%con.Width + 1);
                    if (DateTime.Now.Ticks % 10_000_000 < 5000000) {
                        con.Write('_');
                    }
                }
                con.Flush();
                Thread.Sleep(32);
            }

            con.UseMainScreenBuffer();
            con.Write("Program closing...\n");
            con.Flush();
            Thread.Sleep(200);
        }


        public static void Listener()
        {
            Thread.CurrentThread.IsBackground = true;
            Console.WriteLine(!Console.IsInputRedirected && !Console.IsOutputRedirected);
            if (!Console.IsInputRedirected && !Console.IsOutputRedirected) Console.TreatControlCAsInput = true;
            while (_running) {
                var line = ReadLine();
                if (String.IsNullOrWhiteSpace(line)) continue;
                if (line.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
                    _running = false;
                } else if (line.Equals("render", StringComparison.CurrentCultureIgnoreCase)) {
                    _render = !_render;
                } else {
                    DisplayError("Unknown input: \n" + line);
                }
            }
        }

        public static string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            int num;
            int cursor = 0;

            bool ProcessInput()
            {
                switch (num) {
                    case 3:
                        _running = false;
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
                        _currentInput = null;
                        return true;
                    default:
                        if (num>27) {
                            sb.Insert(cursor, (char) num);
                            cursor++;
                        }
                        break;
                }
                _currentInput = sb.ToString();
                _currentCursor = cursor;
                return false;
            }

            if (Console.IsInputRedirected) {
                while (true) {
                    num = Console.Read();
                    if (ProcessInput()) return sb.ToString();
                }
            } else {
                while (true) {
                    num = Console.ReadKey(true).KeyChar;
                    if (ProcessInput()) return sb.ToString();
                }
            }
        }

        private static void On_ProcessExit(object sender, EventArgs e)
        {
            _running = false;
        }

        private static void DisplayError(string error)
        {
            _errorDisplay = error.Split('\n');
            _errorUntil = DateTime.Now.Ticks + 10_000 * (1000 + error.Length * 80);
            _errorColour = Red;
        }

        private const int VkEscape = 0x1B;
        private const int WmKeydown = 0x0100;
    }
}