using System.Collections.Generic;

namespace Progression.Engine.Core.World.Manager
{
    public class WManManager
    {
        private List<IWManFactory> factories = new List<IWManFactory>();

        public int Count => factories.Count;
    }
}