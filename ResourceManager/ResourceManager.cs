using System;
using System.IO;
using System.Reflection;

namespace Progression.Resources.Manager
{
    public class ResourceManager
    {


        public void LoadPlugins()
        {
            var s = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "lib", "res"));
            if (!s.Exists) s.Create();
            foreach (var file in s.GetFiles("*.json")) {
                Console.WriteLine(file.Name);
                //read json file
            }
            
            
            foreach (var file in s.GetFiles("*.dll")) {
                Console.WriteLine(file.Name);
                var ass = Assembly.LoadFrom(file.FullName);
                //ass.GetManifestResourceStream()
                foreach (var name in ass.GetManifestResourceNames()) {
                    Console.WriteLine(name);
                }
            }
        }
    }
}