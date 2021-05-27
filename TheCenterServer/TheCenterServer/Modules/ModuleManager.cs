using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCenterServer.PModule;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace TheCenterServer
{
    public class ModuleManager : IDisposable
    {
        public static ModuleManager Ins { get; private set; }
        public ModuleManager() { Ins = this; Init(); }
        public WorkspaceManager WorkspaceManager;

        public void Init()
        {
            ScanAssembly(Assembly.GetExecutingAssembly());
            LoadPluginModule();
            WorkspaceManager = new();
            WorkspaceManager.Recovery();
        }
        public void ScanAssembly(Assembly assembly)
        {
            var modules = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ModuleBase)));
            foreach (var m in modules)
            {
                var attr = m.GetCustomAttribute<PModuleAttribute>(false);
                if (attr != null)
                {
                    moduleLibrary.Add(attr.ModuleType, new(attr.ModuleName, m));
                }
            }
        }

        void LoadPluginModule()
        {
            var rootPath = "";
            if (Directory.Exists("plugins"))
            {
                rootPath = "plugins";
            }else if(Directory.Exists(Path.Combine("resources", "plugins")))
            {
                rootPath = Path.Combine("resources", "plugins");
            }else if(Directory.Exists(Path.Combine("resources", "server", "plugins")))
            {
                rootPath = Path.Combine("resources", "server", "plugins");
            }
            var alldir = Directory.GetDirectories(rootPath);
            foreach (var dir in alldir)
            {
                if (File.Exists(Path.Combine(dir, "pmodule.json")))
                {
                    try
                    {
                        var config = JsonSerializer.Deserialize<PluginModuleConfig>(File.ReadAllText(Path.Combine(dir, "pmodule.json")))!;
                        if (config.type == null) config.type = dir;
                        config.dir = dir;
                        moduleLibrary[dir] = new ModuleType(config.name ?? config.type, typeof(PluginModule), config);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public class PluginModuleConfig
        {
            public string interpreter { get; set; }
            public string main { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string dir { get; set; }
            public bool singleton { get; set; }
            public int port { get; set; }
        }

        record ModuleType(string name, Type type, PluginModuleConfig? config = null);

        Dictionary<string, ModuleType> moduleLibrary = new();
        public ModuleBase BuildFrom(string type)
        {
            var mt = moduleLibrary[type];
            var t = mt.type;
            var m = Activator.CreateInstance(t) as ModuleBase;
            if(t == typeof(PluginModule))
            {
                m.config = mt.config;
                m.Type = mt.config.type;
            }
            return m;
        }
        public record ModuleTypeNamePair(string type, string name);
        public List<ModuleTypeNamePair> GetModules()
        {
            return moduleLibrary.Select(kv => new ModuleTypeNamePair(kv.Key, kv.Value.name)).ToList();
        }

        public void Dispose()
        {
            WorkspaceManager.Dispose();
        }

        
    }
}