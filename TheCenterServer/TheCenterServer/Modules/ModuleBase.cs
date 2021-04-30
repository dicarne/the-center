using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TheCenterServer.PModule
{
	public class ModuleBase
	{
		public string ID { get; set; }
		/// <summary>
		/// 注册控件
		/// </summary>
		Dictionary<string, UIControl> controls = new();

		/// <summary>
		/// 注册函数
		/// </summary>
		Dictionary<string, MethodInfo> methods = new();
		public ModuleBase()
		{
			var type = this.GetType();
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
						 BindingFlags.Instance;
			var UIs = type.GetFields(flags).Where(p => p.GetValue(this).GetType().IsSubclassOf(typeof(UIControl)));
			foreach (var ui in UIs)
			{
				var attr = ui.GetCustomAttributes(typeof(UIAttribute), true).FirstOrDefault();
				if (attr != null && attr is UIAttribute ua)
				{
					var ins = ui.GetValue(this) as UIControl;
					ins.Id = ua.name;
					controls.Add(ins.Id, ins);
				}
			}
			var funs = this.GetType().GetMethods(flags).Where(p => p.GetCustomAttributes(typeof(MethodAttribute), true).FirstOrDefault() != null);
			foreach (var item in funs)
			{
				var a = item.GetCustomAttributes(typeof(MethodAttribute), true).FirstOrDefault();
				if (a is MethodAttribute ma)
				{
					methods.Add(ma.name, item);
				}
			}
		}

		public async virtual Task OnLoad()
		{

		}
		public async virtual Task OnDestroy()
		{

		}

		/// <summary>
		/// 提供给前端UI描述信息。
		/// </summary>
		/// <returns></returns>
		public virtual List<UICom> BuildInterface()
		{
			return new List<UICom>();
		}

		/// <summary>
		/// 分发控件事件
		/// </summary>
		/// <param name="control"></param>
		/// <param name="eventname"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object HandleEvent(string control, string eventname, string[] args = null)
		{
			if (controls.TryGetValue(control, out var ins))
			{
				var eventbind = ins.EventBind.FirstOrDefault(e => e.eventname == eventname);
				if (eventbind != null)
				{
					var method = eventbind.method;
					if (methods.TryGetValue(method, out var minfo))
					{
						return minfo.Invoke(this, args);
					}
				}
			}
			return null;
		}

	}

	[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	sealed class PModuleAttribute : Attribute
	{

		public PModuleAttribute(string moduleType)
		{
			ModuleType = moduleType;

		}
		public string ModuleType { get; set; }
	}
}