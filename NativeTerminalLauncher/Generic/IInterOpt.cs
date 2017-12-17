namespace Progression.CCL.Generic
{
    public interface ISizeProvider
    {
        int Height { get; }
        int Width { get; }
        void EnableAnsi();
    }
}