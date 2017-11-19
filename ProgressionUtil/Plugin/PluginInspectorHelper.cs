using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Progression.Util.Plugin
{
    public static class PluginInspectorHelper
    {
        
        public static string Validate<TPlugin, TMan>(FileInfo file, string infoRessourceName, string ressourceFileMainKey,
            params Type[] furtherCheckerTypes)
            where TPlugin : IPlugin<TPlugin, TMan> where TMan : PluginManager<TPlugin, TMan>
        {
            AppDomain domain = null;
            try {
                domain = AppDomain.CreateDomain($"{typeof(TMan).Name} plugin analysis");
                // ReSharper disable once AssignNullToNotNullAttribute
                var c = (StandardValidator) domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                    typeof(StandardValidator).FullName);
                var furtherChecks = new PluginValidator[furtherCheckerTypes.Length];
                for (var i = 0; i < furtherCheckerTypes.Length; i++) {
                    if (!typeof(PluginValidator).IsAssignableFrom(furtherCheckerTypes[i])) throw new ArgumentException($"{nameof(furtherCheckerTypes)}[{i}] must be a subtype of {nameof(PluginValidator)}");
                    
                    
                    // ReSharper disable once AssignNullToNotNullAttribute
                    furtherChecks[i] = (PluginValidator) domain.CreateInstanceAndUnwrap(
                        furtherCheckerTypes[i].Assembly.FullName, furtherCheckerTypes[i].FullName);
                }
                return c.Validate<TPlugin, TMan>(file, infoRessourceName, ressourceFileMainKey, furtherChecks);
            } finally {
                if (domain != null) {
                    AppDomain.Unload(domain);
                }
            }
        }

        public class StandardValidator : MarshalByRefObject
        {
            public string Validate<TPlugin, TMan>(FileInfo file, string infoRessourceName, string ressourceFileMainKey,
                params PluginValidator[] furtherChecks)
                where TPlugin : IPlugin<TPlugin, TMan> where TMan : PluginManager<TPlugin, TMan>
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ReflectionOnlyAssemblyResolve;
                var asm = Assembly.ReflectionOnlyLoadFrom(file.FullName);
                var strings = asm.GetManifestResourceNames();
                foreach (var s in strings) {
                    if (!s.EndsWith("." + infoRessourceName + ".json")) continue;

                    JObject content = JObject.Load(new JsonTextReader(new StreamReader(
                        asm.GetManifestResourceStream(s) ??
                        throw new InvalidOperationException(
                            "Resource file that says it is there can not be found?!"))));

                    var loc = content[ressourceFileMainKey];
                    if (loc == null)
                        throw new InvalidOperationException(
                            $"Json resource file must have key \'{ressourceFileMainKey}\' of type string");
                    if (loc.Type != JTokenType.String)
                        throw new InvalidOperationException(
                            $"Json resource file has key \'{ressourceFileMainKey}\' of type {loc.Type} but it needs to be string");
                    var pluginClass = loc.Value<string>();


                    var classType = asm.GetType(pluginClass);
                    var superType = Type.ReflectionOnlyGetType(typeof(TPlugin).AssemblyQualifiedName ??
                                                               throw new InvalidOperationException("weird"), true,
                        false);
                    var manType = Type.ReflectionOnlyGetType(typeof(TMan).AssemblyQualifiedName ??
                                                             throw new InvalidOperationException("weird"), true,
                        false);
                    if (manType == null || superType == null) throw new InvalidOperationException("weird");
                    if (!superType.IsAssignableFrom(classType))
                        throw new InvalidOperationException(
                            $"{pluginClass} does not implement {typeof(TPlugin).FullName}");
                    if (classType.IsAbstract)
                        throw new InvalidOperationException($"{pluginClass} cannot be abstract");
                    if (classType.GetConstructor(new[] {manType}) == null)
                        throw new InvalidOperationException(
                            $"{pluginClass} must provide a constructor with parameter {typeof(TMan).FullName}");
                    //all standard checks done
                    foreach (var furtherCheck in furtherChecks) {
                        furtherCheck.Validate(classType, superType, manType, content);
                    }
                    //all possible checks done
                    return pluginClass;
                }

                throw new InvalidOperationException($"Assembly does not contain {infoRessourceName}.json file");
            }
            
        }


        public static Assembly ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }
    }
}