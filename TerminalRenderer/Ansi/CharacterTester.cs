namespace Progression.TerminalRenderer.Ansi
{
    public class CharacterTester : ISequenceTraverserPart
    {
        
        private readonly ISequenceTraverserPart _next;
        private readonly char _expected;
        public CharacterTester(char expected, ISequenceTraverserPart next)
        {
            _expected = expected;
            _next = next;
        }

        public AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, object[] data)
        {
            return c != _expected ? null : con.NextPart(_next, b, qo, data);
        }
    }
}