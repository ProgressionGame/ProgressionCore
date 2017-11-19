namespace Progression.Util.Extension
{
    public interface IExtension<TPlugin, in TMan> where TPlugin : IExtension<TPlugin, TMan> where TMan: ExtensionManager<TPlugin, TMan>
    {
        string Name { get; }
        void Load(TMan man);
        void Init(TMan man);
    }
}