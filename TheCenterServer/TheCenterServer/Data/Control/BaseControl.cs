using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheCenterServer
{
	public class UIControl
	{
		public UICom UI { get; protected set; }
		public List<EventBind> EventBind { get; set; }
		public string Id { get => UI.Id; set { UI.Id = value; } }
		public UIControl(List<EventBind> Event)
		{
			EventBind = Event ?? new List<EventBind>();
		}
	}


	public record EventBind(string eventname, string method);

	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	sealed class MethodAttribute : Attribute
	{
		public readonly string name;
		public MethodAttribute(string name)
		{
			this.name = name;
		}

	}
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	sealed class UIAttribute : Attribute
	{
		public readonly string name;
		public UIAttribute(string name)
		{
			this.name = name;
		}

	}
}