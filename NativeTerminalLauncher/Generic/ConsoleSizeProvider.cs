using System;

namespace Progression.CCL.Generic
{
    public class ConsoleSizeProvider : ISizeProvider
    {
        public static readonly ConsoleSizeProvider Instance = new ConsoleSizeProvider();
        
        public int Height => Console.WindowHeight;
        public int Width => Console.WindowWidth;
    }
}