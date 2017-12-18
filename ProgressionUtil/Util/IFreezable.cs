using System.Dynamic;

namespace Progression.Util
{
    public interface IFreezable : IFrozen
    {
        void Freeze();
    }
    public interface IFrozen
    {
        bool IsFrozen { get; }
    }
}