using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Progression.Util
{
    public static class Utils
    {
        static Utils()
        {
            WorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            ExtensionsDirectory = new DirectoryInfo(Path.Combine(WorkingDirectory.FullName, FolderNameExtensions));
            LibraryDirectory = new DirectoryInfo(Path.Combine(ExtensionsDirectory.FullName, FolderNameLibrary));
            ResLibraryDirectory = new DirectoryInfo(Path.Combine(ExtensionsDirectory.FullName, FolderNameResLibrary));
            ModDirectory = new DirectoryInfo(Path.Combine(ExtensionsDirectory.FullName, FolderNameMods));
            ScriptModDirectory = new DirectoryInfo(Path.Combine(ExtensionsDirectory.FullName, FolderNameScriptMods));
            AssetsDirectory = new DirectoryInfo(Path.Combine(WorkingDirectory.FullName, FolderNameAssets));
            
        }
        public const string FolderNameExtensions = "extension";
        public const string FolderNameLibrary = "lib";
        public const string FolderNameResLibrary = "reslib";
        public const string FolderNameMods = "mods";
        public const string FolderNameScriptMods = "scriptmods";
        public const string FolderNameAssets = "assets";


        public  static void Init(){}
        public static DirectoryInfo WorkingDirectory{ get; }
        public static DirectoryInfo ExtensionsDirectory{ get; }
        public static DirectoryInfo LibraryDirectory{ get; }
        public static DirectoryInfo ResLibraryDirectory{ get; }
        public static DirectoryInfo ModDirectory{ get; }
        public static DirectoryInfo ScriptModDirectory{ get; }
        public static DirectoryInfo AssetsDirectory{ get; }

        public static ReleaseType ReleaseType =>
#if DEBUG
            ReleaseType.Debug;
#else
            ReleaseType.Release;
#endif

        public static string ReplaceFileString(string input)
        {
            var sb = new StringBuilder(input);
            sb.Replace("$WorkingDir", WorkingDirectory.FullName);
            sb.Replace("$BuildType", ReleaseType.ToString());
            sb.Replace('/', Path.DirectorySeparatorChar);
            return sb.ToString();
        }

        
        

    }
}