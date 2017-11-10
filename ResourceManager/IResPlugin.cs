namespace Progression.Resources.Manager
{
    public interface IResPlugin
    {
        string Name { get; }
        void Load(ResourceManager man);
        void Init(ResourceManager man);
    }
}