using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Util.Keys;

namespace Progression.TerminalRenderer
{
    public static class Test
    {
        public static IView CreateView(IConsole console)
        {
            
            
            var root = new RootKey("root");

            YieldManager ym = new YieldManagerImpl(new Key(root, "yield"));
            var fw = new FeatureWorld(1, ym);
            fw.Lock();
            
            var world = new TileWorld(fw, 50, 50);
            var view = new WorldView(console, world);
            view.Renderers.Add(new TestCRenderer());
            return view;
        }
    }
}