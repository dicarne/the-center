using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCenterServer
{

	public class Button : UIControl
	{
		public Button(string text, string? onClick = null)
		{
			UI = new UICom()
			{
				type = "button",
			};
			var eventBind = new List<EventBind>();
			if (onClick != null) eventBind.Add(new("onClick", onClick));
			EventBind = eventBind;

			UI.Prop.Add("text", text);
		}
	}

	public enum TextAlign
    {
		center, left, right
    }
	public class Text : UIControl
	{
		public Text(string text, TextAlign textAlign = TextAlign.center) : base()
		{
			UI = new UICom()
			{
				type = "text",
			};
			UI.Prop.Add("text", text);
            switch (textAlign)
            {
                case TextAlign.center:
                    break;
                case TextAlign.left:
					UI.Style.Add("text-align", "left");
					break;
                case TextAlign.right:
					UI.Style.Add("text-align", "right");
					break;
                default:
                    break;
            }
		}
		public Text text(string? newtext)
		{
			UI!.Prop["text"] = newtext;
			return this;
		}
	}
}