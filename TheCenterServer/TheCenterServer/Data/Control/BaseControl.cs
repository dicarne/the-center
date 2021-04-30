using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public class UIControl
    {
        public UICom UI { get; protected set; }
        public List<string> Event => UI.Event;
        public int W => UI.W;
        public int H => UI.H;
        public List<EventBind> EventBind { get; set; }
        public string Id => UI.Id;
        public UIControl(List<EventBind> Event)
        {
            EventBind = Event ?? new List<EventBind>();
        }
    }

    public record EventBind(string eventname, string method);

    [System.AttributeUsage(System.AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    sealed class MethodAttribute : System.Attribute
    {
        public readonly string name;
        public MethodAttribute(string name)
        {
            this.name = name;
        }

    }
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    sealed class UIAttribute : System.Attribute
    {
        public readonly string name;
        public UIAttribute(string name)
        {
            this.name = name;
        }

    }
}