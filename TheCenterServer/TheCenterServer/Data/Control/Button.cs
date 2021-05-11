using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCenterServer
{

    public class Button : UIControl
    {
        public Button(string text, string? onClick = null) : base("button")
        {
            this.text(text);
            this.onClick(onClick);
        }

        [UIParam("显示文本值。")]
        public Button text(string? newtext)
        {
            UI!.Prop["text"] = newtext;
            return this;
        }

        [UIEvent("绑定点击事件。")]
        public Button onClick(string? onclick)
        {
            this.bindEvent("onClick", onclick);
            return this;
        }
    }

    public enum TextAlign
    {
        center, left, right
    }
    public class Text : UIControl
    {
        public Text(string text, TextAlign textAlign = TextAlign.center) : base("text")
        {
            this.text(text);
            this.textAlign(textAlign);
        }

        [UIParam("显示文本值。")]
        public Text text(string? newtext)
        {
            UI!.Prop["text"] = newtext;
            return this;
        }

        [UIStyle("文本对齐方式。可选值：center, left, right", "text-align")]
        public Text textAlign(TextAlign textAlign)
        {
            switch (textAlign)
            {
                case TextAlign.center:
                    UI!.Style["text-align"] = null;
                    break;
                case TextAlign.left:
                    UI!.Style["text-align"] = "left";
                    break;
                case TextAlign.right:
                    UI!.Style["text-align"] = "right";
                    break;
                default:
                    break;
            }
            return this;
        }
    }
}