using System;
using System.Collections.Generic;
using System.Linq;


namespace TheCenterServer
{
    public class UIControl
    {
        public UICom? UI { get; protected set; }
        List<EventBind> m_eventBind = new();
        public List<EventBind> EventBind
        {
            get => m_eventBind;
            set
            {
                m_eventBind = value;
                UI!.Event = m_eventBind.Select(e => e.eventname).ToList();
            }
        }
        public string Id { get => UI.Id; set { UI.Id = value; } }
        public UIControl()
        {
        }
        public static implicit operator UICom(UIControl ctrl)
        {
            return ctrl.UI;
        }
    }


    public record EventBind(string eventname, string method);

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    sealed class MethodAttribute : Attribute
    {
        public readonly string? name;
        public MethodAttribute(string? name = null)
        {
            this.name = name;
        }

    }

    /// <summary>
    /// 标注这个变量是否是UI空间，需要对象继承自UIControl
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    sealed class UIAttribute : Attribute
    {
        public readonly string? name;
        public UIAttribute(string? name = null)
        {
            this.name = name;
        }

    }
}