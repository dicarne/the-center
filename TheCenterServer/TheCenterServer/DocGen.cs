using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public static class DocGen
    {
        public static void Gen(string path)
        {
            if (string.IsNullOrEmpty(path)) path = "apiDoc.json";
            var str = Gen();
            File.WriteAllText(path, str);
        }
        public static string Gen()
        {
            var types = typeof(DocGen).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(UIControl)));
            var docs = new List<OneTypeDesc>();
            foreach (var item in types)
            {
                var d = new OneTypeDesc();
                d.type = item.Name;
                foreach (var m in item.GetMethods())
                {
                    var p = m.GetCustomAttribute<UIParamAttribute>();
                    if (p != null) d.param[p.ParamName ?? m.Name] = p.Desc.UTF8();

                    var s = m.GetCustomAttribute<UIStyleAttribute>();
                    if (s != null) d.style[s.ParamName ?? m.Name] = s.Desc.UTF8();

                    var e = m.GetCustomAttribute<UIEventAttribute>();
                    if (e != null) d.events[e.ParamName ?? m.Name] = e.Desc.UTF8();
                }
                docs.Add(d);
            }
            return JsonSerializer.Serialize(docs);

        }
        class OneTypeDesc
        {
            public string type { get; set; }
            public Dictionary<string, string> param { get; set; } = new();
            public Dictionary<string, string> style { get; set; } = new();
            public Dictionary<string, string> events { get; set; } = new();
        }
    }
}
