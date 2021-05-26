using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public class More : UIControl
    {
        public More(string? text = "more", bool? isshow = true, string? onClick = null) : base("more")
        {
            this.text(text);
            this.isshow(isshow == null ? false : isshow.Value);
            this.onClick(onClick);
        }

        [UIParam("显示文本值。")]
        public More text(string? newtext)
        {
            UI!.prop["text"] = newtext;
            return this;
        }

        [UIParam("是否显示详情值。")]
        public More isshow(bool newtext)
        {
            UI!.prop["isshow"] = newtext ? "true" : "false";
            return this;
        }

        [UIEvent("切换详情。")]
        public More onClick(string? onclick)
        {
            this.bindEvent("onClick", onclick);
            return this;
        }
    }
}
