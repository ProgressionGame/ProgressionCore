using System;
using Progression.Resources.Manager;

namespace Progression.Resources.Decoder.Yaml
{
    public class YamlResPlugin : IResPlugin
    {
        public YamlResPlugin(ResourceDecoderManager man) { }
        public string Name => "YAML1.1";
        public void Load(ResourceDecoderManager man)
        {
            Console.WriteLine("Yaml Load");
        }

        public void Init(ResourceDecoderManager man)
        {
            Console.WriteLine("Yaml Init");
        }
    }
}