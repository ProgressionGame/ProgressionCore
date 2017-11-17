namespace Progression.Util.Plugin
{
    public interface IPlugin<TPlugin, in TMan> where TPlugin : IPlugin<TPlugin, TMan> where TMan: PluginManager<TPlugin, TMan>
    {
        string Name { get; }
        void Load(TMan man);
        void Init(TMan man);
    }
}