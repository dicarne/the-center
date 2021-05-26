using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public class CheckBox: UIControl
    {
        public CheckBox(bool value, string? onChange) : base("checkBox")
        {
            this.Value(value);
            this.onChange(onChange);
        }

        [UIParam("设置是否选中")]
        public CheckBox Value(bool value)
        {
            UI!.Prop["value"] = value ? "true" : "false";
            return this;
        }

        [UIEvent("绑定点击事件。")]
        public CheckBox onChange(string? onchange)
        {
            this.bindEvent("onChange", onchange);
            return this;
        }
    }
}
