using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TheCenterServer.Module
{
    public class ModuleBase
    {
        Dictionary<string, UIControl> controls = new();
        Dictionary<string, System.Reflection.MethodInfo> methods = new();
        public ModuleBase()
        {
            var UIs = this.GetType().GetProperties().Where(p => p.GetType().IsSubclassOf(typeof(UIControl)));
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
            var funs = this.GetType().GetMethods().Where(p => p.GetCustomAttributes(typeof(MethodAttribute), true).FirstOrDefault() != null);
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

        public async Task HandleEvent(string control, string eventname, List<string> args = null)
        {
            if (controls.TryGetValue(control, out var ins))
            {
                var eventbind = ins.EventBind.FirstOrDefault(e => e.eventname == eventname);
                if (eventbind != null)
                {
                    var method = eventbind.method;
                    if (methods.TryGetValue(method, out var minfo))
                    {
                        minfo.Invoke(this, args.ToArray());
                    }
                }
            }
        }
    }
}