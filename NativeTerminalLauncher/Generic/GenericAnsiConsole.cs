using System;
using System.IO;
using System.Text;

namespace Progression.CCL.Generic
{
    public class GenericAnsiConsole : IAnsiConsole
    {
        public TextWriter Writer { get; private set; }
        private bool _consoleWriter;
        private readonly ISizeProvider _interOpt;
        
        public GenericAnsiConsole(TextWriter writer, ISizeProvider interOpt)
        {
            Writer = writer;
            _consoleWriter = false;
            _interOpt = interOpt;
        }

        public GenericAnsiConsole() : this(Console.Out, ConsoleInterOpt.Instance) {
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

        public int Height => _interOpt.Height;
        public int Width => _interOpt.Width;
    }
}