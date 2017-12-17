namespace Progression.TerminalRenderer.Ansi
{
    public class KeyEvent : AnsiInputEvent
    {
        public bool shift { get; }
        public bool control { get; }
        public bool alt { get; }
        public bool num { get; }
        public bool special { get; }
    }
}