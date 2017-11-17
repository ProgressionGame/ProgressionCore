using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Progression.Util.Plugin
{
    public abstract class PluginManager<TPlugin, TMan> : PluginManager where TPlugin : IPlugin<TPlugin, TMan>
        where TMan : PluginManager<TPlugin, TMan>
    {
        public const string RessourceFileMainKey = "Entry";
        public const string LinkerFileMainKey = "Location";
        
        
        
        public override void LoadPlugins()
        {
            if (!Initilized) throw new InvalidOperationException("Plugin manager is not initialized");
            
            
            foreach (var file in Directory.GetFiles("*.json")) {
                Console.WriteLine(file.Name);
                LoadIndirectJson(file).Load((TMan) this);
            }


            foreach (var file in Directory.GetFiles("*.dll")) {
                Console.WriteLine(file.Name);
                LoadFrom(file);
            }
        }

        protected virtual TPlugin LoadIndirectJson(FileInfo file)
        {
            var content = JObject.Load(new JsonTextReader(File.OpenText(file.FullName)));
            var loc = content[LinkerFileMainKey];
            if (loc == null) {
                throw new InvalidOperationException(
                    $"Json file must have key \'{LinkerFileMainKey}\' of type string");
            }
            if (loc.Type != JTokenType.String) {
                throw new InvalidOperationException(
                    $"Json file has key \'{LinkerFileMainKey}\' of type {loc.Type} but it needs to be string");
            }
            return LoadFrom(new FileInfo(Utils.ReplaceFileString(loc.Value<string>())));
        }


        protected virtual TPlugin LoadFrom(FileInfo file)
        {
            var asm = Assembly.ReflectionOnlyLoadFrom(file.FullName);
            string[] strings = asm.GetManifestResourceNames();
            foreach (var s in strings) {
                if (s.EndsWith("." + InfoRessourceName + ".json")) {
                    JObject content = JObject.Load(new JsonTextReader(new StreamReader(
                        asm.GetManifestResourceStream(s) ??
                        throw new InvalidOperationException("Resource file that says it there can not be found?!"))));

                    var loc = content[RessourceFileMainKey];
                    if (loc == null) {
                        throw new InvalidOperationException(
                            $"Json resource file must have key \'{RessourceFileMainKey}\' of type string");
                    }
                    if (loc.Type != JTokenType.String) {
                        throw new InvalidOperationException(
                            $"Json resource file has key \'Entry\' of type {loc.Type} but it needs to be string");
                    }
                    var pluginClass = loc.Value<string>();
                    
                    
                    var classType = asm.GetType(pluginClass);
                    var superType = Type.ReflectionOnlyGetType(typeof(TPlugin).AssemblyQualifiedName ?? throw new InvalidOperationException("weird"), true, false);
                    var manType = Type.ReflectionOnlyGetType(typeof(TMan).AssemblyQualifiedName ?? throw new InvalidOperationException("weird"), true, false);
                    if (manType == null || superType == null) throw new InvalidOperationException("weird");
                    if (!superType.IsAssignableFrom(classType))
                        throw new InvalidOperationException(
                            $"{pluginClass} does not implement {typeof(TPlugin).FullName}");
                    if (classType.IsAbstract) throw new InvalidOperationException($"{pluginClass} cannot be abstract");
                    if (classType.GetConstructor(new[] {manType}) == null) throw new InvalidOperationException(
                        $"{pluginClass} must provide a constructor with parameter {typeof(TMan).FullName}");
                    //all possible checks done. load assembly into app
                    asm = Assembly.LoadFrom(file.FullName); 
                    classType = asm.GetType(pluginClass);
                    return (TPlugin)classType.GetConstructor(new[] {typeof(TMan)})?.Invoke(new object[] {this});
                }
            }

            throw new InvalidOperationException($"Assembly does not contain {InfoRessourceName}.json file");
        }
    }

    public abstract class PluginManager
    {
        static PluginManager()
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += 
                ReflectionOnlyAssemblyResolve;
        }

       

    public static Assembly ReflectionOnlyAssemblyResolve(object sender, 
    ResolveEventArgs args)
    {
            return Assembly.ReflectionOnlyLoad(args.Name);
    }


        public abstract void LoadPlugins();


        public virtual void Init()
        {
            if (Initilized) throw new InvalidOperationException("Cannot be initilized twice");
            if (!Directory.Exists) Directory.Create();
            Initilized = true;
        }

        public abstract DirectoryInfo Directory { get; }
        public abstract string InfoRessourceName { get; }
        public bool Initilized { get; private set; }

    }
}