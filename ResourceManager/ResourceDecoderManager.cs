﻿using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Progression.Util;
using Progression.Util.Extension;

namespace Progression.Resource
{
    public class ResourceDecoderManager : ExtensionManager<IResPlugin, ResourceDecoderManager>
    {
        public override DirectoryInfo Directory => Utils.ResLibraryDirectory;
        public override string InfoRessourceName => "ResLib";

        protected override void InitInternal()
        {
            base.InitInternal();
            AddValidator<ResValidator>();
        }

        private class ResValidator : ExtensionValidator
        {
            public override void Validate(Type extensionType, Type superType, Type managerType, JObject json)
            {
                //this is more or less a test right now. maybe used later for real
                
                var ver = json["Version"];
                if (ver == null)
                    throw new InvalidOperationException(
                        $"Json resource file must have key 'Version' of type string");
                if (ver.Type != JTokenType.String)
                    throw new InvalidOperationException(
                        $"Json resource file has key 'Version' of type {ver.Type} but it needs to be string");
                Console.WriteLine(ver.Value<string>());
            }
        }
    }
}