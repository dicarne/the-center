using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCenterServer.PModule;

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

        record ModuleType(string name, Type type);

        Dictionary<string, ModuleType> moduleLibrary = new();
        public ModuleBase BuildFrom(string type)
        {
            var t = moduleLibrary[type].type;
            return Activator.CreateInstance(t) as ModuleBase;
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