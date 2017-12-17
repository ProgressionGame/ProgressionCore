namespace Progression.TerminalRenderer.Ansi
{
    public class DataInitializer : ISequenceTraverserPart
    {
        private readonly ISequenceTraverserPart _next;
        private readonly int _size;
        public DataInitializer(int size, ISequenceTraverserPart next)
        {
            _size = size;
            _next = next;
        }

        public AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, object[] data)
        {
            return _next.Traverse(con, c, b, qo, new object[_size]);
        }
    }
}