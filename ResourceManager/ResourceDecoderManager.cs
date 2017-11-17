using System;
using System.IO;
using System.Reflection;
using Progression.Util;
using Progression.Util.Plugin;

namespace Progression.Resources.Manager
{
    public class ResourceDecoderManager : PluginManager<IResPlugin, ResourceDecoderManager>
    {
        public override DirectoryInfo Directory => Utils.ResLibraryDirectory;
        public override string InfoRessourceName => "ResLib";
    }
}