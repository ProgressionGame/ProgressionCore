using System;
using Newtonsoft.Json.Linq;

namespace Progression.Util.Extension {
    public abstract class ExtensionValidator : MarshalByRefObject
    {
        public abstract void Validate(Type extensionType, Type superType, Type managerType, JObject json);
    }
}