using System.Collections.Generic;


namespace TheCenterServer
{
    public class Input : UIControl
    {
        public Input(string text, string? onChange = null, string? placeholder = null) : base("input")
        {
            this.text(text);
            this.placeholder(placeholder);
            this.onChange(onChange);
        }

        [UIParam("显示文本值。")]
        public Input text(string? newtext)
        {
            UI!.prop["text"] = newtext;
            return this;
        }

        [UIEvent("绑定点击事件。")]
        public Input onChange(string? onchange)
        {
            this.bindEvent("onChange", onchange);
            return this;
        }

        [UIParam("占位符文本。")]
        public Input placeholder(string? placeholderValue)
        {
            UI!.prop["placeholder"] = placeholderValue;
            return this;
        }
    }
}
