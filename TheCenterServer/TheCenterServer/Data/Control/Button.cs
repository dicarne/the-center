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

        [UIParam("��ʾ�ı�ֵ��")]
        public Button text(string? newtext)
        {
            UI!.Prop["text"] = newtext;
            return this;
        }

        [UIEvent("�󶨵���¼���")]
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

        [UIParam("��ʾ�ı�ֵ��")]
        public Text text(string? newtext)
        {
            UI!.Prop["text"] = newtext;
            return this;
        }

        [UIStyle("�ı����뷽ʽ����ѡֵ��center, left, right", "text-align")]
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