namespace Progression.TerminalRenderer.Ansi
{
    public class SizeEventDataConsumer : ISequenceConsumer
    {
        public AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, object[] data)
        {
            return new SizeEvent((int) data[0], (int) data[1]);
        }
    }
}