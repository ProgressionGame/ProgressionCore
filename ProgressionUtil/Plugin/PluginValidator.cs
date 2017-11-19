using System;
using Newtonsoft.Json.Linq;

namespace Progression.Util.Plugin {
    public abstract class PluginValidator : MarshalByRefObject
    {
        public abstract void Validate(Type pluginType, Type superType, Type managerType, JObject json);
    }
}