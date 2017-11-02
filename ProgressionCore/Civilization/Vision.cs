namespace Progression.Engine.Core.Civilization
{
    public enum Vision
    {
        NotOwned = -1,
        Undiscovered = 0,
        Discovered = 1,
        Visible = 2,
        Owned = 3
    }

    public static class VisionExtension
    {
        public static bool IsNotOwned(this Vision value) => (int)value < 3;
        public static bool IsUndiscovered(this Vision value) => (int)value == 0;
        public static bool IsDiscovered(this Vision value) => (int)value >= 1;
        public static bool IsVisible(this Vision value) => (int)value >= 2;
        public static bool IsOwned(this Vision value) => (int)value == 3;
    }
}