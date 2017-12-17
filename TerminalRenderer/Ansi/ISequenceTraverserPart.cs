namespace Progression.TerminalRenderer.Ansi
{
    public interface ISequenceTraverserPart
    {
        AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, object[] data);
    }
}