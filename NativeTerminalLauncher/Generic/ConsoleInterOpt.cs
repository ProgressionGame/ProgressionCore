using System;

namespace Progression.CCL.Generic
{
    public class ConsoleInterOpt : ISizeProvider
    {
        public static readonly ConsoleInterOpt Instance = new ConsoleInterOpt();

        public int Height => 10;//Console.WindowHeight;
        public int Width => 10;//Console.WindowWidth;
        public void EnableAnsi()
        {
            if (!Console.IsInputRedirected) {
                Console.TreatControlCAsInput = true;
                try {
                    Console.CursorVisible = false;} catch (Exception) {
                    // ignored
                }
            }
        }
    }
}