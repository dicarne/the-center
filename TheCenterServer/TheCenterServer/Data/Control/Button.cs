using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCenterServer
{

	public class Button : UIControl
	{
		public Button(string? onclick = null)
		{
			UI = new UICom()
			{
				type = "button",
			};
			var eventBind = new List<EventBind>();
			if (onclick != null) eventBind.Add(new("onclick", onclick));
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
	}
}