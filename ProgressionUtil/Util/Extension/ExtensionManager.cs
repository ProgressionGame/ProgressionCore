using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Progression.Util.Extension
{
    public abstract class ExtensionManager<TExtension, TMan> : ExtensionManager where TExtension : IExtension<TExtension, TMan>
        where TMan : ExtensionManager<TExtension, TMan>
    {
        public const string RessourceFileMainKey = "Entry";
        public const string LinkerFileMainKey = "Location";

#if DEBUG
        protected ExtensionManager()
        {
            if (!typeof(TMan).IsAssignableFrom(GetType())) 
                throw new ArgumentException($"Generic {nameof(TMan)} is supposed to point to inheriting class");
        }
#endif

        public override void LoadExtensions()
        {
            if (!Initilized) throw new InvalidOperationException("Plugin manager is not initialized");


            foreach (var file in Directory.GetFiles("*.json")) {
                LoadIndirectJson(file).Load((TMan) this);
            }


            foreach (var file in Directory.GetFiles("*.dll")) {
                LoadFrom(file);
            }
        }

        protected virtual TExtension LoadIndirectJson(FileInfo file)
        {
            var content = JObject.Load(new JsonTextReader(File.OpenText(file.FullName)));
            var loc = content[LinkerFileMainKey];
            if (loc == null)
                throw new InvalidOperationException(
                    $"Json file must have key \'{LinkerFileMainKey}\' of type string");
            if (loc.Type != JTokenType.String)
                throw new InvalidOperationException(
                    $"Json file has key \'{LinkerFileMainKey}\' of type {loc.Type} but it needs to be string");
            return LoadFrom(new FileInfo(Utils.ReplaceFileString(loc.Value<string>())));
        }


        protected virtual TExtension LoadFrom(FileInfo file)
        {
            var typeName = ExtensionInspectorHelper.Validate<TExtension, TMan>(
                file, InfoRessourceName, RessourceFileMainKey, Validators);


            //all possible checks done. load assembly into app
            var asm = Assembly.LoadFrom(file.FullName);
            var classType = asm.GetType(typeName);
            return (TExtension) classType.GetConstructor(new[] {typeof(TMan)})?.Invoke(new object[] {this});
        }
    }

    public abstract class ExtensionManager
    {
        private HashSet<Type> _validators = new HashSet<Type>();
        protected Type[] Validators;

        public abstract void LoadExtensions();

        public void Init()
        {
            if (Initilized) throw new InvalidOperationException("Cannot be initialized twice");
            InitInternal();
            Validators = _validators.ToArray();
            _validators = null;
            Initilized = true;
        }

        protected virtual void InitInternal()
        {
            if (!Directory.Exists) Directory.Create();
        }

        protected void AddValidator<TValidator>() where TValidator : ExtensionValidator
        {
            if (Initilized) throw new InvalidOperationException("Cannot add validator after initialization");
            _validators.Add(typeof(TValidator));
        }

        public abstract DirectoryInfo Directory { get; }
        public abstract string InfoRessourceName { get; }
        public bool Initilized { get; private set; }
    }
}