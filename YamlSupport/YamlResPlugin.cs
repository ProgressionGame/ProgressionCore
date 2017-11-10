using System;
using Progression.Resources.Manager;

namespace Progression.Resources.Decoder.Yaml
{
    public class YamlResPlugin : IResPlugin
    {
        public string Name => "YAML1.2";
        public void Load(ResourceManager man)
        {
            Console.WriteLine("Load");
        }

        public void Init(ResourceManager man)
        {
            Console.WriteLine("Init");
        }
    }
}