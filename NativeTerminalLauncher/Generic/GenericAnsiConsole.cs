using System;
using System.IO;
using System.Text;

namespace Progression.CCL.Generic
{
    public class GenericAnsiConsole : IAnsiConsole
    {
        public TextWriter Writer { get; private set; }
        private bool _consoleWriter;
        private readonly ISizeProvider _sizeProvider;
        
        public GenericAnsiConsole(TextWriter writer, ISizeProvider sizeProvider)
        {
            Writer = writer;
            _consoleWriter = false;
            _sizeProvider = sizeProvider;
        }

        public GenericAnsiConsole() : this(Console.Out, ConsoleSizeProvider.Instance) {
            _consoleWriter = true;
            
        }


        public int Write(string str)
        {
            Writer.Write(str);
            return str.Length;
        }

        public void EnableAnsi()
        {
            if (_consoleWriter) {
                var stdout = Console.OpenStandardOutput();
                var conSw = new StreamWriter(stdout, Encoding.ASCII) {AutoFlush = true};
                Writer = conSw;
                _consoleWriter = false;
            }
        }

        public void EnableMouse()
        {
            throw new NotImplementedException();
        }

        public bool SetTitleInteropt(string title) => false;
        public bool SetCursorVisibilityInteropt(bool value) => false;

        public int Height => _sizeProvider.Height;
        public int Width => _sizeProvider.Width;
    }
}