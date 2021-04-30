using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCenterServer.PModule;

namespace TheCenterServer
{
	public class ModuleManager
	{
		public static ModuleManager Ins { get; private set; }
		public ModuleManager() { Ins = this; Init(); }
		public void Init() {
			ScanAssembly(Assembly.GetExecutingAssembly());
		}
		public void ScanAssembly(Assembly assembly) {
			var modules = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ModuleBase)));
			foreach (var m in modules)
			{
				var attr = m.GetCustomAttribute<PModuleAttribute>(false);
				if (attr != null) {
					moduleLibrary.Add(attr.ModuleType, m);
				}
			}
		}
		Dictionary<string, Type> moduleLibrary = new();
		public ModuleBase BuildFrom(string type) {
			var t = moduleLibrary[type];
			return Activator.CreateInstance(t) as ModuleBase;
		}
	}
}