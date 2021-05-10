using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCenterServer
{

	public class Button : UIControl
	{
		public Button(string? onClick = null)
		{
			UI = new UICom()
			{
				type = "button",
			};
			var eventBind = new List<EventBind>();
			if (onClick != null) eventBind.Add(new("onClick", onClick));
			EventBind = eventBind;
		}
	}


	public class Text : UIControl
	{
		public Text(string text) : base()
		{
			UI = new UICom()
			{
				type = "text",
			};
			UI.Prop.Add("text", text);
		}
		public Text text(string? newtext)
		{
			UI!.Prop["text"] = newtext;
			return this;
		}
	}
}