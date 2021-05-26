using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TheCenterServer
{
    public class UIControl
    {
        public UICom? UI { get; protected set; }
        public readonly List<EventBind> EventBind = new();
        public string Id { get => UI!.id; set { UI!.id = value; } }
        public UICom ID(string id)
        {
            UI!.id = id;
            return this;
        }
        public UIControl bindEvent(string eventName, string? method)
        {
            if (method == null)
            {
                EventBind.RemoveAll(e => e.eventname == eventName);
                UI!.eventlist.Remove(eventName);
            }
            else
            {
                var old = EventBind.FindIndex(e => e.eventname == eventName);
                if (old == -1)
                {
                    EventBind.Add(new(eventName, method));
                    UI!.eventlist.Add(eventName);
                }
                else
                {
                    EventBind[old] = new(eventName, method);
                }
            }
            return this;
        }
        public static implicit operator UICom(UIControl ctrl)
        {
            return ctrl.UI!;
        }
        public UIControl(string type)
        {
            UI = new UICom(type);
        }

        [UIParam("行布局时的宽度。")]
        public UIControl Span(int span)
        {
            UI!.prop["span"] = JsonSerializer.Serialize(span);
            return this;
        }

        [UIParam("行flex布局时的宽度。")]
        public UIControl Flex(string flexdata)
        {
            UI!.prop["flex"] = flexdata;
            return this;
        }

        [UIParam("直接设定样式。")]
        public UIControl Style(string stylename, string stylecontent)
        {
            UI!.style[stylename] = stylecontent;
            return this;
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

    [System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    sealed class UIParamAttribute : Attribute
    {
        public readonly string Desc;
        public readonly string? ParamName;
        public UIParamAttribute(string desc, string? paramName = null)
        {
            Desc = desc;
            ParamName = paramName;
        }

    }
    [System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    sealed class UIStyleAttribute : Attribute
    {
        public readonly string Desc;
        public readonly string? ParamName;
        public UIStyleAttribute(string desc, string? paramName = null)
        {
            Desc = desc;
            ParamName = paramName;
        }

    }
    [System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    sealed class UIEventAttribute : Attribute
    {
        public readonly string Desc;
        public readonly string? ParamName;
        public UIEventAttribute(string desc, string? paramName = null)
        {
            Desc = desc;
            ParamName = paramName;
        }

    }
}