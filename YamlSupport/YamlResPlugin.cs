using System;
using Progression.Resources.Manager;

namespace Progression.Resources.Decoder.Yaml
{
    public class YamlResPlugin : IResPlugin
    {
        public YamlResPlugin(ResourceDecoderManager man) { }
        public string Name => "YAML1.2";
        public void Load(ResourceDecoderManager man)
        {
            Console.WriteLine("Loadasdas");
        }

        public void Init(ResourceDecoderManager man)
        {
            Console.WriteLine("Init");
        }
    }
}