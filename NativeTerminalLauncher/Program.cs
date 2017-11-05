using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Progression.CCL.Generic;
using Progression.CCL.Windows;
using Progression.TerminalRenderer;

namespace Progression.CCL
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var platform = Environment.OSVersion.Platform;
            var winNt = platform == PlatformID.Win32NT;
            IConsole con = new NativeConsole(winNt&&!Console.IsOutputRedirected?(IAnsiConsole)new ConhostAnsiConsole(): new GenericAnsiConsole());
            
            con.Init();
            var view = Test.CreateView(con);
            con.UseNewScreenBuffer();
            while (true) {
                view.Render();
                Thread.Sleep(32);

            }
            con.SetCurrentPos(5,5);
                             con.Write("Line1");
                             con.SetCurrentPos(6,con.Width-7);
                             con.Write("Line2");
                             con.Flush();
            Console.Read();

            /*Console.WriteLine(platform);
            con.Init();
            con.SetForegroundColour(255, 0, 0);
            con.Write("a");
            con.SetForegroundColour(128, 0, 0);
            con.Write("a");
            con.SetForegroundColour(64, 0, 0);
            con.Write("a");
            con.SetForegroundColour(32, 0, 0);
            con.Write("a\n");
            con.Bold();
            con.SetForegroundColour(255, 0, 0);
            con.Write("a");
            con.SetForegroundColour(128, 0, 0);
            con.Write("a");
            con.SetForegroundColour(64, 0, 0);
            con.Write("a");
            con.SetForegroundColour(32, 0, 0);
            con.Write("a");
            con.Normal();
            con.CursorUp(2);
            Console.WriteLine("Wrote: " + con.Flush());
            
            con.CursorDown(2);
            con.BlinkSlow();
            con.Write("\nTestTest\n\n");
            con.BlinkOff();
            con.Bold();
            con.Write("TestTest\n");
            con.SetTitle("This is my title!!!!");
            con.Flush();
            
            
            Console.WriteLine("\x1b[36mTEST\x1b[0m");
            
            Console.ReadKey();
            con.UseNewScreenBuffer();
            con.Write("TestTest1\n\n");
            con.Flush();
            Console.ReadKey();
            con.UseNewScreenBuffer();
            con.Write("TestTest2\n\n");
            con.Flush();
            Console.ReadKey();
            con.UseMainScreenBuffer();
            con.Write("done");
            con.Flush();
            Console.ReadKey();
            

/*
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var con = new WindowsConsoleHelper();
            con.Write("\x1b[36mTEST\x1b[0m\n");
            Console.WriteLine(con.ConsoleMode);
            Console.WriteLine();

            con.ConsoleMode |= 0x0004;
            con.Write("\x1b[36mTEST\x1b[0m\n");
            Console.WriteLine(con.ConsoleMode);
            Console.WriteLine();
            
            
            Console.WriteLine(Console.WindowWidth);
            Console.WriteLine(Console.LargestWindowWidth);
            Console.WindowWidth = 50;
            Console.WriteLine(Console.WindowHeight);
            var stdout = Console.OpenStandardOutput();
            var conSw = new StreamWriter(stdout, Encoding.ASCII);
            conSw.AutoFlush = true;
            Console.SetOut(conSw);

            Console.WriteLine("\x1b[36mTEST\x1b[0m");
            Console.WriteLine("done");
            Console.ReadKey();
*/

        }
        
        private const int VK_ESCAPE = 0x1B;
        private const int WM_KEYDOWN = 0x0100;

        
    }
}