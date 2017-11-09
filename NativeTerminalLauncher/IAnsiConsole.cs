namespace Progression.CCL
{
    public interface IAnsiConsole
    {
        int Write(string str);
        void EnableAnsi();
        void EnableMouse();
        bool SetTitleInteropt(string title);
        bool SetCursorVisibilityInteropt(bool value);
        int Height { get; }
        int Width { get; }

    }
}