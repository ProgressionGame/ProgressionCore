using Progression.Util.Keys;

namespace Progression.Resources.Manager
{
    public interface IResourceHook
    {
        void OnHook(IKeyNameable item);
    }
}