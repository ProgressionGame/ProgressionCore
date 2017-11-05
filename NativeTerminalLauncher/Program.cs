using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Progression.CCL.Windows;

namespace Progression.CCL
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var con = new ConsoleManager(new ConhostAnsiConsole());
            Console.WriteLine();
            con.Init();
            con.SetForegroundColour(255, 0, 0);
            con.Write("a");
            con.SetForegroundColour(128, 0, 0);
            con.Write("a");
            con.SetForegroundColour(64, 0, 0);
            con.Write("a");
            con.SetForegroundColour(32, 0, 0);
            con.Write("a");
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