using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheCenterServer
{
    public class Transfer : UIControl
    {
        public Transfer(List<string> avaliable, List<string> all, string? text = null, string? type = null, string? onShow = null, string? onChange = null) : base("transfer")
        {
            this.onChange(onChange);
            this.onShow(onShow);
            this.avaliable(avaliable);
            this.all(all);
            this.type(type);
            this.text(text);
        }

        [UIEvent("绑定点击事件。")]
        public Transfer onChange(string? onchange)
        {
            this.bindEvent("onChange", onchange);
            return this;
        }

        [UIParam("显示文本值。")]
        public Transfer text(string? newtext)
        {
            UI!.Prop["text"] = newtext;
            return this;
        }

        public record ShowData(List<string> ava, List<string>all);

        [UIEvent("绑定详情显示事件。")]
        public Transfer onShow(string? onShow)
        {
            this.bindEvent("onShow", onShow);
            return this;
        }

        [UIParam("选中有效的列表。")]
        public Transfer avaliable(List<string> newtext)
        {
            UI!.Prop["avaliable"] = JsonSerializer.Serialize(newtext);
            return this;
        }

        [UIParam("备选的列表。")]
        public Transfer all(List<string> newtext)
        {
            UI!.Prop["all"] = JsonSerializer.Serialize(newtext);
            return this;
        }

        [UIParam("特殊类型。可选null，local_boards")]
        public Transfer type(string? newtext)
        {
            UI!.Prop["type"] = newtext;
            return this;
        }
    }
}
